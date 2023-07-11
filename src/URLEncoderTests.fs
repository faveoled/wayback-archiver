module URLEncoderTests

open URLEncoder

module URLEncoderTests = 

    // A function to test the urlEncode function
    let testUrlEncode (input: string) (expected: string) =
        // Call the urlEncode function with the input string
        let actual = URLEncoder.urlEncode input
        // Compare the actual output with the expected output
        let result = actual = expected
        // Print the test case and the result
        printfn "Test case: urlEncode \"%s\" = \"%s\"" input expected
        printfn "Result: %b" result
        // Return the result
        result
    let runTests =
        // Some test cases
        testUrlEncode "Hello, world!" "Hello%2C%20world%21" // true
        testUrlEncode "F# is fun" "F%23%20is%20fun" // true
        testUrlEncode "https://en.wikipedia.org/wiki/F_Sharp_(programming_language)" "https%3A%2F%2Fen.wikipedia.org%2Fwiki%2FF_Sharp_%28programming_language%29" // true
        testUrlEncode "This is a test" "This%20is%20a%20test" // true
        testUrlEncode "This is a test with an invalid char \x00" "" // System.ArgumentException: The character '\x00' is not supported by the encoding.
