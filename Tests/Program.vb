Module Program
    Private TC As New TestClient
    Private TS AS New TestServer
    Private TestEndpoint As New Net.IPEndPoint(Net.IPAddress.Parse("0.0.0.0"),55630)
    Sub Main(args As String())
        TC.Endpoint = TestEndpoint
        TS.Endpoint = TestEndpoint
        TS.Test()
        TS.Start()
        TC.TestReusability()
        TS.Stop()
        Console.WriteLine("All tests complete.")
    End Sub
End Module
