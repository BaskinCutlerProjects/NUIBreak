#NUIBreak

By [Robert Baskin](http://www.rsbaskin.com) and [Max Cutler](http://maxcutler.com)

Announcement: [http://blog.rsbaskin.com/post/43940364736/introducing-nuibreak](http://blog.rsbaskin.com/post/43940364736/introducing-nuibreak)

---------------

[According](http://www.cbsnews.com/8301-505143_162-57474678/why-sitting-all-day-is-killing-you/) [to](http://www.nbcnews.com/id/39523298/ns/health-mens_health/t/why-your-desk-job-slowly-killing-you/) [multiple](http://news.yahoo.com/too-much-sitting-kill-study-suggests-200408243.html) [studies](http://www.medicalbillingandcoding.org/sitting-kills/), we're spending more of our time sitting and it's not good for our health.  Sometimes we just need a little reminder to take a quick stretch.

NUIBreak is a program which leverages the power of Microsoft Kinect to remind you to be healthy and stretch regularly and actually checks if you do it! NUIBreak runs quietly in your system tray until a certain user-specified amount of time passes, then blacks out your screen and shows you a couple stretches to do. It won't go away until it uses Kinect to actually see you stretch!

##Usage
When you run NUIBreak, it will start in your system tray. After a certain amount of time (by default 30 minutes), NUIBreak will black out your screen and display a stretch to do. Stand up, make sure your Kinect can see you, and hold the stretch for 5 seconds. NUIBreak will show you one more stretch to do then go away after it's done. It will pop up again 30 minutes later.

By clicking on the tray icon, you can choose whether NUIBreak is enabled or not, how often it should appear, and whether it should run when your system starts up.

If NUIBreak pops up while you're in the middle of something, click the Close button on the bottom right to dismiss it until next time.

##System Requirements and Installation

System Requirements

1. Windows 7 or 8
2. Attached Kinect sensor

Installation Instructions

1. [Install the Kinect for Windows SDK](http://www.microsoft.com/en-us/kinectforwindows/develop/developer-downloads.aspx)
2. Build NUIBreak using the source code from this repository
3. Run NUIBreak.exe

NUIBreak will now be running in your system tray.

##Adding Stretches

Adding your own stretches is easy - you can define them using XML in `stretches.xml`, no code required. Just restart NUIBreak after making your changes. Check out this example:

```xml
<Stretch Name="LeftHandOverHead">
	<Description>Put your left hand above your head.</Description>
	<Rule Type="Compare" Joint1="Head" Joint2="HandLeft" Operator="LT" Axis="Y" />
	<Rule Type="Distance" Joint1="Head" Joint2="HandLeft" Operator="LT" Range="0.25" Axis="X" />
	<Rule Type="Distance" Joint1="Head" Joint2="HandLeft" Operator="LT" Range="0.25" Axis="Z" />
</Stretch>
```

1. Define a stretch and give it a name - make sure it's unique in the file.
2. Add a description - this will show up when your stretch is selected.
3. Add rules for NUIBreak to look for to see if you're doing the stretch. There are two types of rules:  
    a. Compare - Checks whether Joint1 is less than (LT), greater than (GT), or equal to (EQ) Joint2 on a given Axis (X, Y or Z).  
    b. Distance - Checks whether the distance between Joint1 and Joint2 is less than (LT), greater than (GT), or equal to (EQ) a Range on a given Axis (X, Y or Z). Range can be a bit tricky to get right - try experimenting with different values.
	
Valid joint names can be found in Microsoft's documentation for the [JointType enumeration](http://msdn.microsoft.com/en-us/library/microsoft.kinect.jointtype.aspx) in the Kinect SDK.

##Credits

NUIBreak leverages the following libraries:

1. [WPF NotifyIcon](http://www.hardcodet.net/projects/wpf-notifyicon)
2. [Kinect Toolbox](http://kinecttoolbox.codeplex.com)
