Public Partial Class INIFile
    Public Default Property Value(SectionID As String, KeyID As String) As String
        Get
            Return BaseData(SectionID)(KeyID)
        End Get
        Set
            If ContainsSection(SectionID) = False Then AddSection(SectionID)
            If ContainsKey(SectionID, KeyID) = False Then AddKey(SectionID, KeyID)
            BaseData(SectionID)(KeyID) = Value
        End Set
    End Property
    
    Public Property Comment(Line As Int32) As String
        get 
            If CommentData.ContainsKey(Line) = False Then Return ""
            Return CommentData(Line)
        End get
        Set
            If CommentData.ContainsKey(Line) = False Then
                AddComment(Line, Value)
            Else
                CommentData(Line) = Value
            End If
        End Set
    End Property

End Class