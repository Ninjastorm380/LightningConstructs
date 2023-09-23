Public Partial Class INIFile
    Private BaseData As Dictionary(Of String, Dictionary(Of String, String))
    Private CommentData As Dictionary(Of Int32, String)
    Private Shared ReadOnly LF As Char() = vbLf.ToCharArray()
    Private Shared ReadOnly CR As Char() = vbCr.ToCharArray()
    Private Shared ReadOnly CRLF As Char() = vbCrLf.ToCharArray()
    Private Shared ReadOnly KVSplitSymbol As String() = {" = "}
    Private Shared ReadOnly SectionNameStartSymbol As String() = {"[ "}
    Private Shared ReadOnly SectionNameEndSymbol As String() = {" ]"}
    Private Shared ReadOnly CommentStartSymbol As String() = {"# "}
    
    Private Shared ReadOnly KVSplitSymbolCharArray As Char() = KVSplitSymbol(0).ToCharArray()
    Private Shared ReadOnly SectionNameStartSymbolCharArray As Char() = SectionNameStartSymbol(0).ToCharArray()
    Private Shared ReadOnly SectionNameEndSymbolCharArray As Char() = SectionNameEndSymbol(0).ToCharArray()
    Private Shared ReadOnly CommentStartSymbolCharArray As Char() = CommentStartSymbol(0).ToCharArray()
End Class