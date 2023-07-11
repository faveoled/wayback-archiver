module StorageTests

open Fable.Core
open Storage
open Node.Api
open Node

module StorageTests =

  let setup () =
    // Delete the storage directory before each test
    if fs.existsSync(U2.Case1("storage")) then do
      childProcess.execSync("rm -rf ./storage") |> ignore
      Storage.dirsCreated <- false

  let getStorageValShouldReturnNoneIfTheKeyDoesNotExist () =
    // Arrange
    let key = "foo"
    setup ()

    // Act
    let result = Storage.getStorageVal key

    // Assert
    result = None

  let getStorageValShouldReturnSomeValueIfTheKeyExists () =
    // Arrange
    let key = "bar"
    let value = "baz"
    setup ()
    Storage.setStorageVal key value

    // Act
    let result = Storage.getStorageVal key

    // Assert
    result = Some value

  let setStorageValShouldCreateTheStorageDirectoryIfItDoesNotExist () =
    // Arrange
    let key = "qux"
    let value = "quux"
    setup ()

    // Act
    Storage.setStorageVal key value

    // Assert
    fs.existsSync(U2.Case1("storage"))

  let setStorageValShouldCreateAFileWithTheKeyAndValueInTheStorageDirectory () =
    // Arrange
    let key = "corge"
    let value = "grault"
    setup ()
    
    // Act
    Storage.setStorageVal key value

    // Assert
    let path = $"storage/%s{key}.txt"
    fs.existsSync(U2.Case1(path)) && Node.ReadAllText(path) = value

  let setDelayShouldStoreTheDelayAsAStringInTheStorageDirectory () =
    // Arrange
    let delay = 10000
    setup ()

    // Act
    Storage.setDelay delay

    // Assert
    Storage.getStorageVal "DELAY" = Some "10000"

  let getDelayShouldReturnTheDelayAsAnIntFromTheStorageDirectory () =
    // Arrange
    let delay = 20000
    setup ()
    Storage.setDelay delay

    // Act
    let result = Storage.getDelay ()

    // Assert
    result = delay

  let getDelayShouldReturnADefaultValueOf15000IfTheDelayIsNotSet () =
     setup ()
     // Act
     let result = Storage.getDelay ()

     // Assert
     result = 15000

  let setTextToContainShouldStoreTheTextToContainAsAStringInTheStorageDirectory () =
     // Arrange
     let text = "bing.com"
     setup ()

     // Act
     Storage.setTextToContain text

     // Assert
     Storage.getStorageVal "TEXT_CONTAIN" = Some text

  let getTextToContainShouldReturnTheTextToContainAsAStringFromTheStorageDirectory () =
     // Arrange
     let text = "yahoo.com"
     setup ()
     Storage.setTextToContain text

     // Act
     let result = Storage.getTextToContain ()

     // Assert
     result = text

  let getTextToContainShouldReturnADefaultValueOfGoogleComIfTheTextToContainIsNotSet () =
     setup ()
     // Act
     let result = Storage.getTextToContain ()

     // Assert
     result = "google.com"

  let setTargetWebsiteShouldStoreTheTargetWebsiteAsAStringInTheStorageDirectory () =
     // Arrange
     let url = "https://bing.com"
     setup ()

     // Act
     Storage.setTargetWebsite url

     // Assert
     Storage.getStorageVal "TARGET_WEBSITE" = Some url

  let getTargetWebsiteShouldReturnTheTargetWebsiteAsAStringFromTheStorageDirectory () =
     // Arrange
     let url = "https://yahoo.com"
     setup ()
     Storage.setTargetWebsite url

     // Act
     let result = Storage.getTargetWebsite ()

     // Assert
     result = url

  let getTargetWebsiteShouldReturnADefaultValueOfHttpsGoogleComIfTheTargetWebsiteIsNotSet () =
      setup ()
      // Act 
      let result = Storage.getTargetWebsite ()

      // Assert 
      result = "https://google.com"

  let getKeyShouldReturnAFormattedStringBasedOnTheUrl () =
      // Arrange 
      let url1 = "https://example.com/foo/bar"
      let url2 = "http://example.org:8080/baz/qux"

      // Act 
      let key1 = Storage.getKey url1 
      let key2 = Storage.getKey url2 

      // Assert 
      key1 = "URL_https___example_com_foo_bar" && key2 ="URL_http___example_org_8080_baz_qux"

  let isUrlDoneShouldReturnTrueIfTheUrlIsMarkedAsDoneInTheStorageDirectory () =
      // Arrange 
      let url = "https://example.com"
      setup ()
      Storage.setUrlDone url 

      // Act 
      let result = Storage.isUrlDone url 

      // Assert 
      result = true

  let isUrlDoneShouldReturnFalseIfTheUrlIsNotMarkedAsDoneInTheStorageDirectory () =
      // Arrange 
      let url = "https://example.com"
      setup ()

      // Act 
      let result = Storage.isUrlDone url 

      // Assert 
      result = false

  let setUrlDoneShouldStoreAPlusSignAsTheValueForTheUrlKeyInTheStorageDirectory () =
      // Arrange 
      let url = "https://example.com"
      setup ()

      // Act 
      Storage.setUrlDone url 

      // Assert 
      Storage.getStorageVal (Storage.getKey url) = Some "+"
