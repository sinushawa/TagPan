macroScript RobFastTag
category:"RobBC"
toolTip:"Rob FastTag"
Icon:#("RBC3",1)
(
	fn ForceRedrawAuto sender args =
	(
		if toggle == true then
		(
			redrawViews()
		)
	)
	fn startAuto =
	(
		autoForm = dotNetObject "MaxCustomControls.MaxForm"
		autoForm.name="tag panel"
		autoForm.size = dotNetObject "System.Drawing.Size" 320 30
		maxHandlePointer=(Windows.GetMAXHWND())
		sysPointer = DotNetObject "System.IntPtr" maxHandlePointer
		maxHwnd = DotNetObject "MaxCustomControls.Win32HandleWrapper" sysPointer
		autoPan = dll.createInstance("TagPan.FastHost")
		autoPan.CreateAutoCompleteSource(treePan)
		dotNet.addEventHandler autoPan "ForceRedraw" ForceRedrawAuto
		dotNet.setLifetimeControl autoPan #dotnet
		autoForm.controls.add autoPan
		autoForm.Text = "tags panel"
		autoForm.maximizeBox=false
		autoForm.minimizeBox=true
		autoForm.showIcon=false
		autoForm.topmost=false
		autoForm.showInTaskbar=false
		BorderStyles= dotNetClass "System.Windows.Forms.FormBorderStyle"
		autoForm.FormBorderStyle = BorderStyles.None
		FromStart= dotNetClass "System.Windows.Forms.FormStartPosition"
		autoForm.StartPosition = FromStart.CenterScreen
		autoForm.Location = dotNetObject "System.Drawing.Point" (mouse.screenpos.x) (mouse.screenpos.y)
		dotNet.setLifetimeControl autoForm #dotnet
		autoForm.show(maxHwnd)
		autoPan.LinkParent()
	)
	startAuto()
) 