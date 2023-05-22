module LambdaInterpreterImpl

type LambdaTerm =
    | LambdaAbstraction of string * LambdaTerm
    | LambdaApplication of LambdaTerm * LambdaTerm
    | Variable of string


let rec getFreeVars term (free:Set<string>) =
    match term with
    | Variable(var) -> free.Add(var)
    | LambdaAbstraction(var, termInLambda) -> (getFreeVars termInLambda free).Remove(var) 
    | LambdaApplication (term1, term2) -> (getFreeVars term1 free) + (getFreeVars term2 free)

let rec substitution term1 term2 instead =
    match term1 with
    | Variable var -> if var = instead then term2 else Variable var
    | LambdaApplication (termInApp1, termInApp2) -> LambdaApplication (substitution termInApp1 term2 instead, substitution termInApp2 term2 instead)
    | LambdaAbstraction (var, term) ->
        match var with
        // term doesn't contain free var named `instead`
        | v when v = instead -> LambdaAbstraction(var, term)
        // substitution contains abstraction name
        | v when not ((getFreeVars term2 Set.empty).Contains(var) && (getFreeVars term Set.empty).Contains(instead)) ->
            LambdaAbstraction(var, substitution term term2 instead)
        // find `instead` in body
        | _ ->
           let newVar = ((getFreeVars term2 Set.empty) + (getFreeVars term Set.empty)).MaximumElement + "'"
           let newTerm = Variable(newVar)
           let termWithNewVar = substitution term newTerm var
           LambdaAbstraction(newVar, substitution termWithNewVar term2 instead)

let rec reduceOnce term =
    let rec reduceOnce' term =
        match term with
        | LambdaAbstraction (param, body) ->
            (match reduceOnce' body with
             | None -> None
             | Some reducedOnce -> Some (LambdaAbstraction (param, reducedOnce)))
        | LambdaApplication (LambdaAbstraction (param, body), toSubstitute) ->
            Some (substitution body toSubstitute param)
        | LambdaApplication (left, right) ->
            (match reduceOnce' left with
             | Some reducedLeft -> Some (LambdaApplication (reducedLeft, right))
             | None ->
                 (match reduceOnce' right with
                  | Some reducedRight -> Some (LambdaApplication (left, reducedRight))
                  | None -> None))
        | Variable _ -> None
    reduceOnce' term

let rec reduce term =
    let rec reduce' term =
        match reduceOnce term with
        | None -> term
        | Some reduced -> reduce' reduced
    reduce' term