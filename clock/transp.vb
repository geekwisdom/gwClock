'/********************************************************************************
'File: ClockForm.vb
'@(#) Purpose: This Is the main clock form that acts like a widget
'@(#) that can be moved around on your desktop.
'@(#)  Special thanks to Chris Wilby for providing this sample of transparency
'@(#) See: https : //blog.geekwisdom.org/2019/05/the-most-dangerous-software-on-internet.html
'**********************************************************************************
'Written By: Chris Wilby
'Created: Oct 25, 2019
'********************************************************************************/

Public Class transp



    Sub make_transparent(theform As Form)
        If theform.TransparencyKey = theform.BackColor Then
            theform.TransparencyKey = Color.Plum
        Else
            theform.TransparencyKey = theform.BackColor
        End If
    End Sub

    Sub toggle_transparent(theform As Form, TransParentOn As Boolean)
        If TransParentOn Then
            theform.TransparencyKey = theform.BackColor
        Else
            theform.TransparencyKey = Color.Plum

        End If
    End Sub


    Private Sub SetBackgroundColorToolStripMenuItem_Click(theform As Form)
        Dim MyDialog As New ColorDialog()
        MyDialog.AllowFullOpen = False ' Keeps the user from selecting a custom color.
        MyDialog.ShowHelp = True  ' Allows the user to get help. (The default is false.)

        ' Update the text box color if the user clicks OK 
        If (MyDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            ' Button2.Text = MyDialog.Color.ToString
        End If
        theform.BackColor = MyDialog.Color
    End Sub

    Sub TrackBar1_Scroll(theform As Form, tb As TrackBar)
        theform.Opacity = tb.Value / 10 '+ 0.01

    End Sub

    Sub toggle_borders(theform As Form, borderOn As Boolean)
        If borderOn Then
            theform.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
        Else
            theform.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        End If
    End Sub


End Class
