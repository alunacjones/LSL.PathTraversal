# Merging settings 

Sometimes it is useful to merge settings from the top of a hierarchy
down to the deeper initial folder such as with `.gitignore` files.

The following example uses `Newtonsoft.Json` to merge json documents to provide a final
settings file.

!!! note
    We reverse the found files list so that we start merging from the file found highest
    in the hierarchy i.e. the one nearest the root of the file system.

## Assumed folder structure

!!! warning
    This structure is assuming that we are at he root of the file system
    and will therefore not encounter any further files except for the ones
    we specified. 
    
    This would not normally be the case and we would want
    to either limit how far up the tree we go or take into account further files.
```
├── my-app-settings.json 
├── inner1
│   ├── my-app-settings.json
|   ├── inner2
│   |   ├── my-app-settings.json
```

## `/my-app-settings.json` content

```json
{
    "Integer": 12
}
```

## `/inner1/my-app-settings.json` content

```json
{
    "String": "inner1-string",
    "StringArray": [
        "inner1-item1",
        "inner1-item2"
    ]        
}
```

## `/inner1/inner2/my-app-settings.json` content

```json
{
    "Integer": 99,
    "StringArray": [
        "inner2-item1",
        "inner2-item2",
        "inner2-item3"
    ]        
}
```

## Code sample

!!! note
    Clicking the <span class="md-icon md-try-it-out-inline" ></span> button in the top right
    of the code sample will take you to a fuller dotnet fiddle to run the code.
    
```csharp { data-fiddle="L7pYDc"}
// This path will be relative to the current folder
var files = TraversePath.Enumerate("inner1/inner2")
    .Select(di => di.GetFileInfo("my-app-settings.json"))   
    .Reverse();

var currentValue = JObject.Parse("{}");

foreach (var file in files)
{
    var content = File.ReadAllText(file.FullName);
    currentValue.Merge(
        JObject.Parse(content), 
        new() { MergeArrayHandling = MergeArrayHandling.Replace });
}

var result = currentValue.ToObject<MergeData>();

/*
    result will be equivalent to:

    new MergeData 
    {
        Integer = 99,
        String = "inner1-string",
        StringArray = [
            "inner2-item1",
            "inner2-item2",
            "inner2-item3"
        ]
    }    
*/
```

