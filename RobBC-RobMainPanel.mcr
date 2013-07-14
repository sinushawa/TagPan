macroScript RobMainPanel
category:"RobBC"
toolTip:"Rob Tag treePanel"
Icon:#("RBC",1)
(
	fn RecursiveChildren _dnode _parent =
	(
		hold = selection as array
		clearselection()
		for i=0 to _dnode.Children.Count-1 do
		(
			obj = getnodebyname _dnode.Children.Item[i].Value.label
			if obj != undefined then delete obj
			child = _dnode.Children.Item[i]
			_vnode = dummy()
			_vnode.name="*"+child.Value.label
			
			if _parent != undefined then
			(
				_vnode.parent=_parent
			)
			hide _vnode
			RecursiveChildren child _vnode
		)
	)
	fn SaveEvent sender args =
	(
		for i=0 to args.Value.Key.Count-1 do
		(
			objs=#()
			for j=0 to args.Value.Key.Item[i].objects.Count-1 do
			(
				obj =GetAnimByHandle args.Value.Key.Item[i].objects.Item[j]
				if obj!=undefined AND (IsValidNode obj) then
				(
					append objs obj
				)
			)
			selectionSets[args.Value.Key.Item[i].label] = objs
		)
	)
	fn SaveEventC =
	(
		args = treePan.GetDataForSave()
		for i=0 to args.Value.Key.Count-1 do
		(
			objs=#()
			for j=0 to args.Value.Key.Item[i].objects.Count-1 do
			(
				obj =GetAnimByHandle args.Value.Key.Item[i].objects.Item[j]
				if obj!=undefined AND (IsValidNode obj) then
				(
					append objs obj
				)
			)
			selectionSets[args.Value.Key.Item[i].label] = objs
		)
	)	
	fn PostNodeCloned =
	(
		args = callbacks.notificationParam()
		originals = args[1]
		cloned = args[2]
		type = args[3]
		if type != 0 then
		(
			for i=1 to originals.Count do
			(
				clonedNodes = #()
				nbO=GetHandleByAnim originals[i]
				nbC = GetHandleByAnim cloned[i]
				append clonedNodes nbo
				append clonedNodes nbc
				append clonesHolder clonedNodes
			)
			--treePan.CloneNode clonesHolder[1] clonesHolder[2]
		)
	)
	fn ClonedPostProcess =
	(
		print "process"
		for i=1 to clonesHolder.Count do
		(
			treePan.CloneNode clonesHolder[i][1] clonesHolder[i][2]
		)
		clonesHolder=#()
	)
	
	fn PreOpenEventC =
	(
		treePan.ClearTree()
	)
	fn PostOpenEventC =
	(
		holdsel = selection as array
		clearselection()
		ToSendBack = treePan.CreateTemplateData()
		for j=1 to getNumNamedSelSets() do
		(
			ToAdd = treePan.CreateKeyPair()
			ToAdd.key=selectionSets[j].name
			ObjList= treePan.CreateObjList()
			for i=1 to (getNamedSelSetItemCount j) do
			(
				if (getNamedSelSetItem j i) != undefined then
				(
					ObjList.Add(GetHandleByAnim (getNamedSelSetItem j i))
				)
			)
			ToAdd.objects=ObjList
			ToSendBack.Add(ToAdd)
		)
		treePan.LoadData(ToSendBack)
		for i in holdsel do
		(
			selectmore i
		)
	)

	fn onClosing sender args =
	(
		setINISetting iniPath "PanelPosition" "X" (tagForm.location.X as string)
		setINISetting iniPath "PanelPosition" "Y" (tagForm.location.Y as string)
		setINISetting iniPath "PanelPosition" "Width" (tagForm.Width as string)
		setINISetting iniPath "PanelPosition" "Height" (tagForm.Height as string)
		
		callbackItem = undefined
		callbacks.removeScripts id:#tagPanel
		gc light:true
	)

	fn CallbackDeleted ev nds =
	(
		disableSceneRedraw()
		local tColl = DotNetObject "System.Collections.Generic.List`1[System.Int32]"
		tColl.Clear()
		for nd in nds do
		(
			tColl.Add(nd)
		)
		treePan.DeleteObjects(tColl)
		enableSceneRedraw()
	)
	fn CallbackSelChanged ev nds =
	(
		-- check if coming from cloning
		if clonesHolder != undefined AND clonesHolder.Count>0 then
		(
			ClonedPostProcess()
		)
		--
		
		local tColl = DotNetObject "System.Collections.Generic.List`1[System.Int32]"
		tColl.Clear()
		local nColl = DotNetObject "System.Collections.Generic.List`1[System.string]"
		nColl.Clear()
		for nb in selection do
		(
			res = IsValidNode nb
			if res==true then
			(
				hdw=GetHandleByAnim nb
				tColl.Add(hdw)
				nColl.Add(nb.name)
			)
		)
		treePan.selectedObjects.ReplaceRange tColl
		treePan.selectedObjectsNames.ReplaceRange nColl
	)

	fn selectionEvent sender args =
	(
		disableSceneRedraw()
		clearselection()
		treePan.selectedObjects.Count
		for i=0 to i=args.Value.Count-1 do
		(
			obj =GetAnimByHandle args.Value.Item[i]
			if obj != undefined do
			(
				selectMore obj
			)
		)
		enableSceneRedraw()
	)


	fn renameObjectEvent sender args =
	(
		obj =GetAnimByHandle args.Value.Key
		if obj!= undefined AND (IsValidNode obj) then
		(
			_currentName = trimright obj.name "_1234567890"
			if _currentName!=args.Value.Value then
			(
				obj.name = uniqueName (args.Value.Value+"_")
			)
		)
	)
	fn renameTagEvent sender args =
	(
		for i=0 to args.Value.Count-1 do
		(
			try
			(
				old = args.Value.Item[i].Key
				new = args.Value.Item[i].Value
				hold= for j in selectionsets[old] collect j
				deleteItem selectionsets old
				selectionsets[new]=hold
			)
			catch
			(
			)
		)
	)
	fn deleteTagEvent sender args =
	(
		for i=0 to args.Value.Count-1 do
		(
			try
			(
				old = args.Value.Item[i]
				deleteItem selectionsets old
			)
			catch
			(
			)
		)
	)
	fn ForceRedrawPan sender args =
	(
		if toggle == true then
		(
			redrawViews()
		)
	)


	fn init =
	(
		tagForm = dotNetObject "MaxCustomControls.MaxForm"
		treePan = dll.createInstance("TagPan.TagPanel")
		tagForm.size = dotNetObject "System.Drawing.Size" 210 600
		tagForm.location = dotNetObject "System.Drawing.Point" 200 300
		treePan.size = dotNetObject "System.Drawing.Size" 190 540
		
		
		existFile = doesFileExist iniPath
		if existFile==true then
		(
			FromStart= dotNetClass "System.Windows.Forms.FormStartPosition"
			tagForm.StartPosition = FromStart.Manual
			TLX = (getINISetting iniPath "PanelPosition" "X") as number
			TLY = (getINISetting iniPath "PanelPosition" "Y") as number
			tagForm.location = dotNetObject "System.Drawing.Point" TLX TLY
			width = (getINISetting iniPath "PanelPosition" "Width") as number
			height = (getINISetting iniPath "PanelPosition" "Height") as number
			tagForm.size = dotNetObject "System.Drawing.Size" width height
			treePan.size = dotNetObject "System.Drawing.Size" (width-20) (height-60)
		)
		
		enableAccelerators = false
		clearListener()
		global callbackItem = NodeEventCallback delay:10 deleted:CallbackDeleted added:CallbackSelChanged selectionChanged:CallbackSelChanged nameChanged:CallbackSelChanged
		
		tagForm.name="tag panel"
		maxHandlePointer=(Windows.GetMAXHWND())
		CoP = windows.getChildHWND 0 "Command Panel" parent:#max
		GmT = windows.getChildHWND 0 "Ribbon" parent:#max
		CoPPointer= DotNetObject "System.IntPtr" CoP[1]
		GmTPointer= DotNetObject "System.IntPtr" GmT[1]
		sysPointer = DotNetObject "System.IntPtr" maxHandlePointer
		maxHwnd = DotNetObject "MaxCustomControls.Win32HandleWrapper" sysPointer
		
		--groupBox END
			
		-- treeView START
		
		treePan.Left=3
		treePan.Top = 18
		AnchorStyles= dotNetClass "System.Windows.Forms.AnchorStyles"
		AnchorValue =dotNet.combineEnums AnchorStyles.Top AnchorStyles.Bottom AnchorStyles.Left AnchorStyles.Right
		treePan.Anchor= AnchorValue
		dotNet.addEventHandler treePan "SelectionEvent" selectionEvent
		dotNet.addEventHandler treePan "RenameObjectEvent" renameObjectEvent
		dotNet.addEventHandler treePan "RenameTagEvent" renameTagEvent
		dotNet.addEventHandler treePan "DeleteTagEvent" deleteTagEvent
		dotNet.addEventHandler treePan "ForceRedraw" ForceRedrawPan
		dotNet.addEventHandler treePan "SaveEvent" SaveEvent
		dotNet.setLifetimeControl treePan #dotnet
		tagForm.controls.add treePan
		tagForm.Text = "tags panel"
		tagForm.maximizeBox=false
		tagForm.minimizeBox=true
		tagForm.showIcon=false
		tagForm.topmost=false
		tagForm.showInTaskbar=false
		dotNet.addEventHandler tagForm "Closing" onClosing
		dotNet.setLifetimeControl tagForm #dotnet
		tagForm.show(maxHwnd)
		treePan.RegisterDockableTo(CoPPointer)
		treePan.RegisterDockableTo(GmTPointer)
		holdsel = selection as array
		clearselection()
		ToSendBack = treePan.CreateTemplateData()
		for j=1 to getNumNamedSelSets() do
		(
			ToAdd = treePan.CreateKeyPair()
			ToAdd.key=selectionSets[j].name
			ObjList= treePan.CreateObjList()
			for i=1 to (getNamedSelSetItemCount j) do
			(
				ObjList.Add(GetHandleByAnim (getNamedSelSetItem j i))
			)
			ToAdd.objects=ObjList
			ToSendBack.Add(ToAdd)
		)
		treePan.LoadData(ToSendBack)
		for i in holdsel do
		(
			selectmore i
		)
		callbacks.addScript #postNodesCloned "PostNodeCloned()" id:#tagPanel --creates problems with instance more than one
		
		callbacks.addScript #filePreSave "SaveEventC()" id:#tagPanel
		callbacks.addScript #filePreOpen "PreOpenEventC()" id:#tagPanel
		callbacks.addScript #filePostOpen "PostOpenEventC()" id:#tagPanel
		
	)

 



init()
)