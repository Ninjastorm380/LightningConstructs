Imports System.Text

Public Partial Class INIFile
    Public Sub New()
        BaseData = New Dictionary(Of String, Dictionary(Of String, String))
        CommentData = New Dictionary(Of Integer,String)()
    End Sub

    Public Shared Function Load(Path as String) As INIFile
        Dim RawString As String = Text.Encoding.ASCII.GetString(IO.File.ReadAllBytes(Path))
        Return New INIFile With { .BaseData = UnwrapData(RawString), .CommentData = UnwrapComments(RawString)}
    End Function

    Public Sub Save(Path as String)
        IO.File.WriteAllBytes(Path, Text.Encoding.ASCII.GetBytes(Wrap(BaseData, CommentData)))
    End Sub

    Public Function ContainsSection(SectionID As String) As Boolean
        Return BaseData.ContainsKey(SectionID)
    End Function

    Public Function ContainsKey(SectionID As String, KeyID As String) As Boolean
        Return BaseData(SectionID).ContainsKey(KeyID)
    End Function

    Public Function GetSectionIDAt(Index As Int32) As String
        Return BaseData.Keys(Index)
    End Function

    Public Function GetKeyIDAt(SectionID As String, Index As Int32) As String
        Return BaseData(SectionID).Keys(Index)
    End Function

    Public Sub AddSection(SectionID As String)
        If ContainsSection(SectionID) = False Then BaseData.Add(SectionID, New Dictionary(Of String,String))
    End Sub

    Public Sub AddKey(SectionID As String, KeyID As String)
        If ContainsSection(SectionID) = False Then AddSection(SectionID)
        If ContainsKey(SectionID, KeyID) = False Then BaseData(SectionID).Add(KeyID, String.Empty)
    End Sub

    Public Sub RemoveSection(SectionID As String)
        If ContainsSection(SectionID) = True Then BaseData.Remove(SectionID)
    End Sub

    Public Sub RemoveKey(SectionID As String, KeyID As String)
        If ContainsSection(SectionID) = False Then Return
        If ContainsKey(SectionID, KeyID) = True Then BaseData(SectionID).Remove(KeyID)
    End Sub

    Public Sub AddComment(Line as Int32, Comment As String)
        CommentData.Add(Line, Comment)
    End Sub

    Public Sub RemoveComment(Line as Int32)
        CommentData.Remove(Line)
    End Sub

    Private Shared Function Wrap(Data As Dictionary(Of String, Dictionary(Of String, String)),
                                 Comments As Dictionary(Of Int32, String)) As String
        If Data.Count = 0 Then Return ""

        Dim Entries As New List(Of String)
        For Index = 0 To Data.Count - 1
            If Data.Keys(Index) <> "" Then
                Entries.Add(SectionNameStartSymbolCharArray + Data.Keys(Index) + SectionNameEndSymbolCharArray)
                For ChildIndex = 0 To Data(Data.Keys(Index)).Count - 1
                    Entries.Add(
                        Data(Data.Keys(Index)).Keys(ChildIndex) + KVSplitSymbolCharArray +
                        Data(Data.Keys(Index)).Values(ChildIndex))
                Next
                Entries.Add(vbCrLf)
            End If
        Next

        If Comments.Count > 0
            For Index = 0 To Comments.Count - 1
                Entries.Insert(Comments.Keys(Index), CommentStartSymbolCharArray + Comments.Values(Index))
            Next
        End If

        Dim RawBuilder As New StringBuilder
        For Each Item In Entries
            RawBuilder.Append(Item)
            If Item <> vbCrLf Then RawBuilder.Append(vbCrLf)
        Next
        Dim Raw As String = RawBuilder.ToString().TrimEnd(CRLF)
        Debug.Print(Raw)
        Return Raw
    End Function

    Private Shared Function UnwrapData(Raw As String) As Dictionary(Of String, Dictionary(Of String, String))
        Dim Items As String() = Raw.Split(CRLF, StringSplitOptions.None)
        Dim Name As String = String.Empty
        Dim Item As String
        Dim Data as New Dictionary(Of String, Dictionary(Of String, String))
        For Counter = 0 To Items.Length - 1
            Item = Items(Counter).Trim()

            If MatchStartingChars(Item, SectionNameStartSymbolCharArray, SectionNameStartSymbolCharArray.Length) = True
                Name = Item.TrimStart(SectionNameStartSymbolCharArray).TrimEnd(SectionNameEndSymbolCharArray)
                If Data.ContainsKey(Name) = False And Name <> "" Then
                    Data.Add(Name, New Dictionary(Of String,String))
                End If
            ElseIf MatchStartingChars(Item, CommentStartSymbolCharArray, CommentStartSymbolCharArray.Length) = True

            Else
                If Item <> "" And Name <> "" Then
                    Dim ItemPair As String() = Item.Split(KVSplitSymbol, 2, StringSplitOptions.None)
                    ItemPair(0) = ItemPair(0).Trim()
                    ItemPair(1) = ItemPair(1).Trim()

                    If Data(Name).ContainsKey(ItemPair(0)) = True
                        Data(Name)(ItemPair(0)) = ItemPair(1)
                    Else
                        Data(Name).Add(ItemPair(0), ItemPair(1))
                    End If
                End If
            End If
        Next
        Return Data
    End Function

    Private Shared Function UnwrapComments(Raw As String) As Dictionary(Of Int32, String)
        Dim Items As String() = Raw.Split(CRLF, StringSplitOptions.RemoveEmptyEntries)
        Dim Item As String
        Dim Data as New Dictionary(Of Int32, String)
        For Counter = 0 To Items.Length - 1
            Item = Items(Counter).Trim()
            If MatchStartingChars(Item, CommentStartSymbolCharArray, CommentStartSymbolCharArray.Length) = True
                Data.Add(Counter, Item.TrimStart(CommentStartSymbolCharArray))
            End If
        Next
        Return Data
    End Function

    Private Shared Function MatchStartingChars(A As String, B As String, Length As Int32) As Boolean
        If A.Length < Length Then Return False
        If B.Length < Length Then Return False

        Dim Result As Boolean = True
        For Index = 0 To Length - 1
            If A(Index) <> B(Index) Then Result = False
        Next
        Return Result
    End Function

    Public Overrides Function ToString As String
        Return Wrap(BaseData, CommentData).TrimEnd(CRLF)
    End Function
End Class