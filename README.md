# Making a Transparent Desktop Clock

![ScreenShot](https://github.com/geekwisdom/gwClock/blob/master/assets/clock_sample.png?raw=true)

#Introducton

In this sample I will be demonstrating some of the idea of creating an application/gadget which you can display on your screen using Visual Basic .NET.  In previous windows programming languages, many of these techniques required special Windows API calls, but it is far easier now to create your own simple gadgets that you can easily place on your desktop using existing .NET components and managed code.

#Background

Recently a collegue was playing around with transparency in VB.NET, and he happened to show me his transparent clock application, which brought me back to the old days of [Windows 'desktop gadgets'](https://en.wikipedia.org/wiki/Microsoft_Gadgets). What I like most about his demonstration was the use of the matrix background, which was just too cool for [my geeky self to pass](https://www.youtube.com/watch?v=MeabQjpkMFY) up.

#Design

The overall goal is to create a simple window which will display the time in digitial format, we need a way to be able to easily re-size and place the clock in an area of our screen to our liking. We also want to be able to adjust the clock so that we can still see information behind the clock for a nice transparency. Clicking the the clock time, we would like the title bar/borders to be hidden, and upon saving and re-opening we would like the clock to remember its size, position and transparency.

#Transparency

Transparency is a technology that has been around for a long time. Also called ["Chromakey"](https://en.wikipedia.org/wiki/Chroma_key) or GreenScreen. The idea is to have software replace a specific color (eg: Green) with the a different background of your choosing.  In Windows Forms, this basically works the same way, by setting the transparency key property to a specific color, all items that are that color will be replaced by the background.

    theform.BackColor = Color.Black
    theform.TransparencyKey = Color.Black
   
   Any items that are black will effectivly be replaced by the background making the item transparent.
   
  The degree of replacement is controled by the Opacity Value. A value beteen 0 and 1 where (1 = Fully Opaque, and 0 = Fully Transparent).  To allow the user the ability to control the level of transparency I use a simple trackbar, and as the trackbar changes so does the opacity
  
    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
            ' cls.TrackBar1_Scroll(Me, sender)
            Me.Opacity = sender.Value / 100 '+ 0.01
            Me.Text = Me.Opacity
    End Sub

*Tip*:  The minimim trackbar value if allowed at "0" means that the user can mike the entire screen fully transparent. this *includes* the trackbar making it very difficult to adjust what you cannot see. So let's set a minimum of 20 or so to prevent this from happening.

#Borders and Title

The make it really look 'gadget' like we want to hide the borders around it. In .NET 4.5 and greater this is very easy

      Sub toggle_borders(theform As Form, borderOn As Boolean)
        If borderOn Then
            theform.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
        Else
            theform.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        End If
      End Sub

This intoduces a bit of a problem though because without borders, we cannot move or resize the screen, and putting a button on the screen will look a little strange in our 'gadget view'. To solve this we will add the ability to toggle the borders on and off when the user clicks on the clock. We use a 'clickstate' global to the form to toggle the transparency and borders on and off

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        SetTrans()
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

Note as well here that we are using *Me.TopMost* to keep our gadget on top of other windows.

Finally we want to save the size and position of our gadget when closing and re-opening.  To do this we convert the position by saving the TOP and LEFT properties of the form in a point format (X,Y) and the size by storing the width and height. We also store the transparency value and last clickstate

  Private Sub Form1_Close(sender As Object, e As EventArgs) Handles MyBase.FormClosed
        If Me.WindowState = FormWindowState.Normal Then
            WriteSetting("ClockLocation", Me.Left.ToString + "," + Me.Top.ToString)
            WriteSetting("ClockSize", Me.Width.ToString + "," + Me.Height.ToString)
            WriteSetting("ClockTransp", Me.TrackBar1.Value.ToString)
            WriteSetting("ClockState", ClickState.ToString)
        End If

    End Sub

We read these settings in again upon form load

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

So there you have it. Your very own transparent clock. If you are interested in only the executable you can find it here:

Enjoy !
