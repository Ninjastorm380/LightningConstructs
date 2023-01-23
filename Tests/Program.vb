Imports Lightning
Module Program
    Private spintest As new SpinGate(Of String)
    Private TC As New TestClient
    Private TS AS New TestServer
    Private TestEndpoint As New Net.IPEndPoint(Net.IPAddress.Parse("0.0.0.0"),55630)
    Sub Main(args As String())
        Console.WriteLine("Creating new .conf formatted file...")
        
        Dim NewConfiguration As New ConfigFile
        NewConfiguration.Name = "Test Configuration"
        NewConfiguration("A") = "Apple"
        NewConfiguration("B") = "Apple"
        NewConfiguration("C") = "Apple"
        Console.WriteLine(NewConfiguration.ToString())
        Console.WriteLine("")
        
        Console.WriteLine("Saving new .conf formatted file...")
        NewConfiguration.Save("./test.conf")
        Console.WriteLine("")
        
        Console.WriteLine("Reading in new .conf formatted file...")
        Dim LoadedConfiguration As ConfigFile = ConfigFile.Load("./test.conf")
        Console.WriteLine(LoadedConfiguration.ToString())
        Console.WriteLine("")
        
        Console.WriteLine("Modifying loaded .conf formatted file...")
        LoadedConfiguration.Name = "Modified Test Configuration"
        LoadedConfiguration("C") = "Orange"
        LoadedConfiguration("D") = "Orange"
        LoadedConfiguration("E") = "Orange"
        LoadedConfiguration("F") = "Orange"
        LoadedConfiguration("G") = "Orange = best"
        Console.WriteLine(LoadedConfiguration.ToString())
        Console.WriteLine("")
        
        Console.WriteLine("Saving modified .conf formatted file...")
        LoadedConfiguration.Save("./test.conf")
        Console.WriteLine("")
        
        Console.WriteLine("Reading in modified .conf formatted file...")
        Dim ModdedConfiguration As ConfigFile = ConfigFile.Load("./test.conf")
        Console.WriteLine(ModdedConfiguration.ToString())
        Console.WriteLine("")
        
        Console.WriteLine("Testing SpinLock. this should timeout after 3000ms...")
        Dim duration as Stopwatch = Stopwatch.StartNew()
        Try
            spintest.Lock(3000)
        Catch ex as Exception
            Console.WriteLine(ex.Message & " - duration: " & duration.ElapsedMilliseconds & "ms")
        End Try
        duration.Stop()
        
        If duration.ElapsedMilliseconds < 3000 Then 
            Console.WriteLine("Spingate lock ended prematurely! Took " & duration.ElapsedMilliseconds & "ms, expected 3000ms!")
            Console.WriteLine("Halting tests...")
            Exit Sub
        End If
        
        duration = Nothing
        
        TC.Endpoint = TestEndpoint
        TS.Endpoint = TestEndpoint
        TS.Test()
        TS.Start()
        TC.TestReusability()
        TS.Stop()
        Console.WriteLine("All tests complete.")
    End Sub
End Module
