macroScript RobFastPaste
category:"RobBC"
toolTip:"Rob Tag Copy Tags"
Icon:#("RBC3",1)
(
	/*
	cursorPath = (symbolicPaths.getPathValue"$icons")+@"\eyeDropper.cur"
	cursors = dotNetClass "System.Windows.Forms.Cursors"
	hand_move = dotNetObject "System.Windows.Forms.Cursor" cursorPath
	cursors.Hand.Current = hand_move
	*/
	TagHolder = pickObject message:"select node containing tags to copy"
	hdw=GetHandleByAnim TagHolder
	_tags=treePan.getObjectTags(hdw)
	treePan.PasteTagsToSelection hdw
)