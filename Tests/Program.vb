Module Program
    Private TC As New TestClient
    Private TS AS New TestServer
    Private TestEndpoint As New Net.IPEndPoint(Net.IPAddress.Parse("0.0.0.0"),55630)
    Sub Main(args As String())
        TC.Endpoint = TestEndpoint
        TS.Endpoint = TestEndpoint
        TS.TestReusability()
        TS.Start()
        TC.TestReusability()
        TS.Stop()
    End Sub
End Module
