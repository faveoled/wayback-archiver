module Tests

open Fable.Mocha
open URLEncoder

let arithmeticTests =
    testList "Arithmetic tests" [
        test "plus works" {
            Expect.equal (1 + 1) 2 "plus"
        }

        test "Test for falsehood" {
            Expect.isFalse (1 = 2) "false"
        }

        testAsync "Test async code" {
            let! x = async { return 21 }
            let answer = x * 2
            Expect.equal 42 answer "async"
        }
    ]

let toBytesTests =
    testList "To bytes tests" [
        test "empty string returns empty array" {
            Expect.equal (URLEncoder.toBytes "") [||] "empty"
        }

        test "ASCII string returns correct array" {
            Expect.equal (URLEncoder.toBytes "Hello") [|72uy; 101uy; 108uy; 108uy; 111uy|] "ASCII"
        }

        test "Unicode string returns correct array" {
            Expect.equal (URLEncoder.toBytes "Привет") [|208uy; 159uy; 209uy; 128uy; 208uy; 184uy; 208uy; 178uy; 208uy; 181uy; 209uy; 130uy;|] "Unicode"
        }
    ]
    
let urlEncodeTests =
    testList "Url encode tests" [
        test "empty string returns empty string" {
            Expect.equal (URLEncoder.urlEncode "") "" "empty"
        }

        test "spaces are replaced with plus signs" {
            Expect.equal (URLEncoder.urlEncode "Hello world") "Hello+world" "space"
        }

        test "special characters are percent-encoded" {
            Expect.equal (URLEncoder.urlEncode "!@#$%^&*()") "%21%40%23%24%25%5E%26%2A%28%29" "special"
        }
        
        test "unicode characters are encoded correctly" {
            Expect.equal (URLEncoder.urlEncode "Привет") "%D0%9F%D1%80%D0%B8%D0%B2%D0%B5%D1%82" "unicode"
        }
    ]

Mocha.runTests toBytesTests |> ignore
Mocha.runTests arithmeticTests |> ignore
Mocha.runTests urlEncodeTests |> ignore