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

Imports System.Configuration


Public Class ClockForm
    Dim TransparentControl As New transp
    Dim ClickState As Boolean = True


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label1.Text = Format(TimeOfDay, "h:mm tt")
    End Sub

    Private Sub Form1_Close(sender As Object, e As EventArgs) Handles MyBase.FormClosed
        If Me.WindowState = FormWindowState.Normal Then
            WriteSetting("ClockLocation", Me.Left.ToString + "," + Me.Top.ToString)
            WriteSetting("ClockSize", Me.Width.ToString + "," + Me.Height.ToString)
            WriteSetting("ClockTransp", Me.TrackBar1.Value.ToString)
            WriteSetting("ClockState", ClickState.ToString)
        End If

    End Sub
    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Label1.Height = Me.Height
        Label1.Width = Me.Width
        'Label1.Font = New Font(Label1.Font.FontFamily, Me.Height / 2.5)
        Label1.Font = New Font(Label1.Font.FontFamily, Me.Height / 3)
        PictureBox1.Height = Me.Height
        PictureBox1.Width = Me.Width
    End Sub



    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        ' cls.TrackBar1_Scroll(Me, sender)
        Me.Opacity = sender.Value / 100 '+ 0.01
        Me.Text = Me.Opacity

    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Parent = PictureBox1
        TrackBar1.Minimum = 20
        Dim ClockLocation As String
        Dim ClockSize As String
        Dim ClockTrans As String
        Dim ClockState As String

        ClockLocation = ReadSetting("ClockLocation", "")
        ClockSize = ReadSetting("ClockSize", "")
        ClockTrans = ReadSetting("ClockTransp", "")
        ClockState = ReadSetting("ClockState", "")
        If ClockLocation <> "" Then Me.Location = ParsePoint(ClockLocation)
        If ClockSize <> "" Then Me.Size = ParseSize(ClockSize)
        If ClockState <> "" Then
            ClickState = Boolean.Parse(ClockState)
            If ClickState Then SetTrans()
        End If

            If ClockTrans <> "" Then
            TrackBar1.Value = Int32.Parse(ClockTrans)
            Me.Opacity = TrackBar1.Value / 100 '+ 0.01
            Me.Text = Me.Opacity
        Else
            TrackBar1.Value = 86
            Me.Opacity = TrackBar1.Value / 100 '+ 0.01
            Me.Text = Me.Opacity
        End If

        TrackBar1.Refresh()



    End Sub





    Private Sub SetTrans()
        If ClickState Then
            'TransparentControl.toggle_transparent(Me, False)
            TransparentControl.toggle_borders(Me, False)
            TrackBar1.Visible = False
            TransparencyBar.Visible = False
            Me.TopMost = True
            Me.ShowInTaskbar = False
            ClickState = False
        Else
            'TransparentControl.toggle_transparent(Me, True)

            TrackBar1.Visible = True
            TransparencyBar.Visible = True
            Me.TopMost = False
            Me.ShowInTaskbar = True
            TransparentControl.toggle_borders(Me, True)
            Me.Text = "GeekWisdom Clock"
            ClickState = True
        End If

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        SetTrans()
    End Sub

    Private Sub WriteSetting(key As String, value As String)
        Try

            Dim configFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings

            If settings(key) Is Nothing Then
                settings.Add(key, value)
            Else
                settings(key).Value = value
            End If

            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
        Catch __unusedConfigurationErrorsException1__ As ConfigurationErrorsException
            Console.WriteLine("Error writing app settings")
        End Try
    End Sub

    Private Function ReadSetting(ByVal key As String, defaultval As String) As String
        Try
            Dim appSettings = ConfigurationManager.AppSettings
            Dim result As String = If(appSettings(key), defaultval)
            Return result
        Catch __unusedConfigurationErrorsException1__ As ConfigurationErrorsException
            Console.WriteLine("Error reading app settings")
        End Try
    End Function

    Private Function ParsePoint(PointString) As Point
        Dim coords As String() = PointString.Split(","c)
        Dim point As Point = New Point(Integer.Parse(coords(0)), Integer.Parse(coords(1)))
        Return point
    End Function

    Private Function ParseSize(PointString) As Size
        Dim coords As String() = PointString.Split(","c)
        Dim Sz As Size = New Size(Integer.Parse(coords(0)), Integer.Parse(coords(1)))
        Return Sz
    End Function
End Class
