#r @"packages/FAKE.3.9.8/tools/FakeLib.dll"


open Fake
open Fake.AssemblyInfoFile
open System.Text.RegularExpressions
open System.Xml
open System.Xml.Xsl
open System.Text
open System.Text.RegularExpressions
open System.IO
open System.Reflection


let version = getBuildParamOrDefault "version" "1.0.0.0"
let configuration = getBuildParamOrDefault "configuration" "Debug"



let appName = "MingleTransitionMonitor"
let buildDir = "./Service/bin/"
let buildConfigDir = buildDir @@ configuration
let testOutputDir = "./testOutput/"
let deployDir = "./_deploy/"
let nuspecFile = deployDir @@ "MingleTransitionMonitor.Service.nuspec"


let generateNunitReport (nUnitXmlFilename : string) (outputFilename : string) (projectName : string) =
    let settings = new XsltSettings(false,true)
    settings.EnableDocumentFunction <- true
    let xslt = new XslCompiledTransform()
    xslt.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),  "NUnit2Report/NUnit.xsl"), settings, new XmlUrlResolver())
    let doc = new XmlTextReader(nUnitXmlFilename)
    use memoryStream = new MemoryStream()
    use textWriter = new XmlTextWriter(memoryStream, new UTF8Encoding(false))
    use writer = System.Xml.XmlWriter.Create(textWriter, xslt.OutputSettings)
    writer.WriteStartDocument()
    let args = new XsltArgumentList()
    args.AddParam("fake.project.name", "", projectName)
    xslt.Transform(doc, args, writer)
    let outputDoc = new XmlDocument()
    let encoding = new UTF8Encoding(false)
    memoryStream.ToArray()
    |> encoding.GetString
    |> outputDoc.LoadXml
    outputDoc.Save outputFilename



Target "Clean" (fun _ ->
    !! "**/*.sln"
      |> MSBuild null "Clean" [ "Configuration", configuration ]
      |> Log "Clean-Output: "
    CleanDir testOutputDir
)

Target "Compile" (fun _ ->
    trace ("Compiling with configuration of " + configuration)
    !! "**/*.sln"
      |> MSBuild null "Build" [ "Configuration", configuration ]
      |> Log "Compile-Output: "
)

Target "Build" (fun _ ->
    Run "Compile"
    Run "UnitTests"
)

Target "UnitTests" (fun _ ->
    let nUnitXmlFile = testOutputDir @@ "results/TestResults.xml"
    let outputHtmlFile = testOutputDir @@ "results/TestResults.html"
    try
        ensureDirectory (testOutputDir @@ "results")
        !! (testOutputDir + "/*Tests*.dll") 
            |> NUnit (fun p ->
                {p with
                    DisableShadowCopy = true
                    StopOnError = false
                    OutputFile = nUnitXmlFile })
    finally
        //generateNunitReport nUnitXmlFile outputHtmlFile (appName + " v" + version)
        Log "Fix NUnit report"
)


Target "Package" (fun _ ->
    let appVersion = appName + "." + version
    let workingDir = buildDir @@ configuration
    ensureDirectory deployDir
    CleanDir deployDir

    (!! ("**/*.*")).SetBaseDirectory(workingDir).ButNot("*vshost*") |> CreateZip workingDir (deployDir+appVersion+".zip") appVersion DefaultZipLevel false

)


Target "PackageNuget" (fun _ ->
    ensureDirectory deployDir
    CleanDir deployDir
    let workingDir = buildDir @@ configuration
    let appVersion = appName + "." + version

    // This is not working-- the folder structure is flattened but it should be preserved ex config/log4net.config    
    (!! ("**/*.*")).SetBaseDirectory(workingDir).ButNot("*vshost*") |> CopyTo deployDir

    let assemblyConfgs = (!! ("/*.exe.config")).SetBaseDirectory(deployDir)
    
    if assemblyConfgs |> Seq.length>1 then raise (System.ApplicationException("More than one assembly config found! Cannot guess assembly config"))

    let assemblyConfig = assemblyConfgs |> Seq.exactlyOne |> filename
    let assemblyName = assemblyConfig.Replace(".config","")
    !! (deployDir + "/app.*.config")
        |> Seq.iter (fun (x)-> 
            let newName=(Regex.Replace(x,"app", assemblyName, RegexOptions.IgnoreCase))
            Rename newName x
        ) 

    
)

Target "UpdateVersion" (fun _ ->
    CreateCSharpAssemblyInfo "./SolutionInfo.cs"
       [Attribute.Product "MingleTransitionMonitor"
        Attribute.Company "Mingle Transition Monitor Project"
        Attribute.Version version
        Attribute.FileVersion version]
)

"Clean" 
    ==> "UpdateVersion" 
    ==> "Compile" 

RunTargetOrDefault "Build"

