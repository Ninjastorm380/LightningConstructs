Imports System
Imports System.Numerics
Imports System.Text

Module Program
    Private ReadOnly TestClient As New TestClient
    Private ReadOnly TestServer AS New TestServer
    Private ReadOnly TestEndpoint As Net.IPEndPoint = Net.IPEndPoint.Parse("0.0.0.0:55630")

    Sub Main(Args As String())
        TestKeyExchange()
        TestGovernor()
        TestSpinGate()
        TestINIFile()
        TestSocket()
        
        Console.WriteLine("Press enter to close.")
        Console.ReadLine()
    End Sub
    
    Private Sub TestKeyExchange()
        Dim Result As BigInteger = Nothing
        Dim LocalKey As BigInteger = Nothing, LocalSeed As BigInteger = Nothing
        Dim RemoteKey As BigInteger = Nothing, RemoteSeed As BigInteger = Nothing
    
        Dim MaxLocalPublicKeySize As Int32 = 0
        Dim MaxRemotePublicKeySize As Int32 = 0
        Dim MinLocalPublicKeySize As Int32 = Int32.MaxValue
        Dim MinRemotePublicKeySize As Int32 = Int32.MaxValue
        Dim LocalPublicKeySize As Int32 = 0
        Dim RemotePublicKeySize As Int32 = 0
        
        Dim IntervalTimespan As TimeSpan = Timespan.FromMilliseconds(1000)
        Dim EndTimespan As TimeSpan = Timespan.FromMilliseconds(5000)
        
        Console.WriteLine("Testing KeyExchange")
        
        Dim IntervalStopwatch As Stopwatch = Stopwatch.StartNew()
        Dim TimeoutStopwatch As Stopwatch = Stopwatch.StartNew()
        
        
        Do
            Result = Nothing
            LocalKey = Nothing : LocalSeed = Nothing
            RemoteKey = Nothing : RemoteSeed = Nothing
            
            
            LambdaThread.Start(Sub()
                Dim Local As New KeyExchange
                LocalKey = Local.PublicKey
                Do Until RemoteKey <> Nothing
                    LambdaThread.Yield()
                Loop
                LocalSeed = Local.Derive(RemoteKey)
            End Sub, "Local-Side Diffie-Hellman Cryptographic Sync Test Thread")
            
            
            LambdaThread.Start(Sub()
                Dim Remote As New KeyExchange
                RemoteKey = Remote.PublicKey
                Do Until LocalKey <> Nothing
                    LambdaThread.Yield()
                Loop
                RemoteSeed = Remote.Derive(LocalKey)
            End Sub, "Remote-Side Diffie-Hellman Cryptographic Sync Test Thread")
            
            
            Do Until (LocalSeed <> Nothing And RemoteSeed <> Nothing) = True
                LambdaThread.Yield()
            Loop
            Result = LocalSeed - RemoteSeed
            LocalPublicKeySize = LocalKey.ToByteArray().Length
            RemotePublicKeySize = RemoteKey.ToByteArray().Length
            
            If LocalPublicKeySize > MaxLocalPublicKeySize Then MaxLocalPublicKeySize = LocalPublicKeySize
            If RemotePublicKeySize > MaxRemotePublicKeySize Then MaxRemotePublicKeySize = RemotePublicKeySize
            If LocalPublicKeySize < MinLocalPublicKeySize Then MinLocalPublicKeySize = LocalPublicKeySize
            If RemotePublicKeySize < MinRemotePublicKeySize Then MinRemotePublicKeySize = RemotePublicKeySize
            
            
            If IntervalStopwatch.Elapsed >= IntervalTimespan Or Result <> BigInteger.Zero Then
                Console.WriteLine("  Local Public Key Byte Length:          " & LocalPublicKeySize)
                Console.WriteLine("  Remote Public Key Byte Length:         " & RemotePublicKeySize)
                Console.WriteLine("  Maximum Local Public Key Byte Length:  " & MaxLocalPublicKeySize)
                Console.WriteLine("  Maximum Remote Public Key Byte Length: " & MaxRemotePublicKeySize)
                Console.WriteLine("  Minimum Local Public Key Byte Length:  " & MinLocalPublicKeySize)
                Console.WriteLine("  Minimum Remote Public Key Byte Length: " & MinRemotePublicKeySize)
                Console.WriteLine("  Local Shared Key Seed Byte Length:     " & LocalSeed.ToByteArray().Length)
                Console.WriteLine("  Remote Shared Key Seed Byte Length:    " & RemoteSeed.ToByteArray().Length)
                Console.WriteLine("  Shared Key Difference Delta:           " & Result.ToString())
                Console.WriteLine("")
                IntervalStopwatch.Restart()
            End If
            

        Loop Until (Result <> BigInteger.Zero Or TimeoutStopwatch.Elapsed >= EndTimespan) = True
        TimeoutStopwatch.Reset()
        IntervalStopwatch.Reset()
    End Sub
    
    Private Sub TestGovernor()
        Console.WriteLine("Testing Governor...")
        Dim EndTimespan As TimeSpan = Timespan.FromMilliseconds(5000)
        Dim IntervalTimespan As TimeSpan = Timespan.FromMilliseconds(1000)
        Dim TimeoutStopwatch As Stopwatch = Stopwatch.StartNew()
        Dim IntervalStopwatch As Stopwatch = Stopwatch.StartNew()
        Dim TimeoutGovernor As New Governor(60)
        Do Until TimeoutStopwatch.Elapsed >= EndTimespan
            If IntervalStopwatch.Elapsed >= IntervalTimespan
                Console.WriteLine("  delta at frequency 60: " & TimeoutGovernor.Delta.TotalMilliseconds & "ms")
                IntervalStopwatch.Restart()
            End If
            TimeoutGovernor.Limit()
        Loop
        TimeoutGovernor.Frequency = 1
        TimeoutStopwatch.Restart()
        IntervalStopwatch.Restart()
        Do Until TimeoutStopwatch.Elapsed >= EndTimespan
            If IntervalStopwatch.Elapsed >= IntervalTimespan
                Console.WriteLine("  delta at frequency 1: " & TimeoutGovernor.Delta.TotalMilliseconds & "ms")
                IntervalStopwatch.Restart()
            End If
            TimeoutGovernor.Limit()
        Loop
        
        TimeoutGovernor.Frequency = 1000
        TimeoutStopwatch.Restart()
        IntervalStopwatch.Restart()
        Do Until TimeoutStopwatch.Elapsed >= EndTimespan
            If IntervalStopwatch.Elapsed >= IntervalTimespan
                Console.WriteLine("  delta at frequency 1000: " & TimeoutGovernor.Delta.TotalMilliseconds & "ms")
                IntervalStopwatch.Restart()
            End If
            TimeoutGovernor.Limit()
        Loop
        TimeoutStopwatch.Reset()
        IntervalStopwatch.Reset()
        Console.WriteLine("")
    End Sub
    
    Private Sub TestINIFile()
        Console.WriteLine("Testing INIFile...")
        Dim NewConfiguration As New INIFile
        NewConfiguration("Test Configuration", "A") = "Apple"
        NewConfiguration("Test Configuration", "B") = "Apple"
        NewConfiguration("Test Configuration", "C") = "Apple"
        
        Console.WriteLine("  saving new .ini formatted file...")
        Console.WriteLine(Offset(NewConfiguration.ToString(), 4))
        NewConfiguration.Save("./test.conf")
        Console.WriteLine("")
        
        Console.WriteLine("  reading in new .ini formatted file...")
        Dim LoadedConfiguration As INIFile = INIFile.Load("./test.conf")
        Console.WriteLine(Offset(LoadedConfiguration.ToString(), 4))
        Console.WriteLine("")
        
        Console.WriteLine("  modifying loaded .ini formatted file...")
        LoadedConfiguration("Modified Test Configuration", "A") = "Apple"
        LoadedConfiguration("Modified Test Configuration", "B") = "Apple"
        LoadedConfiguration("Modified Test Configuration", "C") = "Orange"
        LoadedConfiguration("Modified Test Configuration", "D") = "Orange"
        LoadedConfiguration("Modified Test Configuration", "E") = "Orange"
        LoadedConfiguration("Modified Test Configuration", "F") = "Orange"
        LoadedConfiguration("Modified Test Configuration", "G") = "Orange = best"
        LoadedConfiguration.Comment(4) = "new comment"
        LoadedConfiguration.Comment(0) = "new comment"
        LoadedConfiguration.RemoveComment(4)
        Console.WriteLine(Offset(LoadedConfiguration.ToString(), 4))
        Console.WriteLine("")
        
        Console.WriteLine("  saving modified .ini formatted file...")
        Console.WriteLine(Offset(LoadedConfiguration.ToString(), 4))
        LoadedConfiguration.Save("./test.conf")
        Console.WriteLine("")
        
        Console.WriteLine("  reading in modified .ini formatted file...")
        Dim ModdedConfiguration As INIFile = INIFile.Load("./test.conf")
        Console.WriteLine(Offset(ModdedConfiguration.ToString(), 4))
        Console.WriteLine("")
    End Sub
    
    Private Sub TestSocket()
        TestClient.Endpoint = TestEndpoint
        TestServer.Endpoint = TestEndpoint
        Console.WriteLine("Testing server socket...")
        TestServer.Listen()
        TestServer.Deafen()
        TestServer.Listen()
        TestServer.Deafen()
        Console.WriteLine("")
        
        Console.WriteLine("Testing client socket...")
        TestServer.Listen()
        TestClient.Connect()
        TestClient.Disconnect()
        TestClient.Connect()
        TestClient.Disconnect()
        TestServer.Deafen()
        Console.WriteLine("")
    End Sub
    
    Private Sub TestSpinGate()
        Console.WriteLine("Testing SpinGate. This should timeout after 1000ms...")
        Dim SpinTest As New SpinGate With { .Timeout = TimeSpan.FromMilliseconds(1000) }, Duration as Stopwatch = Stopwatch.StartNew()
        SpinTest.Lock() : Duration.Stop() : Console.WriteLine("  lock duration: " & Duration.Elapsed.TotalMilliseconds & "ms")
        Console.WriteLine("")
    End Sub
    
    Private Function Offset(Input As String, Spaces As Int32) As String
        Dim Lines As String() = Input.Split(vbCrLf)
        
        Dim PaddingBuilder As New StringBuilder : For Index = 0 To Spaces - 1
            PaddingBuilder.Append(" ")
        Next : Dim Padding As String = PaddingBuilder.ToString()
        
        Dim ResultBuilder As New StringBuilder : For Index = 0 To Lines.Length - 1
            ResultBuilder.Append(Padding) : ResultBuilder.Append(Lines(Index))
            If Index <> Lines.Length - 1 Then ResultBuilder.Append(vbCrLf)
        Next : Return ResultBuilder.ToString()
    End Function
    
End Module
