macroScript RobFastInfo
category:"RobBC"
toolTip:"Rob Tag info overlay"
Icon:#("RBC2",1)
(
	fn toggleTagInfo = 
	(
		
		toggle = not toggle
		if toggle == true then
		(
			unRegisterRedrawViewsCallback showObjectNames
			fn showObjectNames=
			(
				
				aColor = color 210 200 180
				gw.setColor #line aColor
				gw.setTransform(Matrix3 1)
				for o in selection where not o.isHidden do
				(
					hdw=GetHandleByAnim o
					_tags=treePan.getObjectTags(hdw)
					if _tags.Count>0 then
					(
						ph=gw.hTransPoint o.pos
						maxlength =0
						ph=gw.hTransPoint o.pos
						lineArr=#([ph.x+3,ph.y+3,0], [ph.x+50, ph.y+50,0])
						gw.hMarker [ph.x,ph.y,0] #circle color:aColor
						gw.hPolyline lineArr false
					)
					for i=0 to (_tags.Count-1) do
					(
						_clength=gw.getTextExtent _tags.Item[i]
						if _clength.x>maxlength then
						(
							maxlength=_clength.x
						)
					)
					for i=0 to (_tags.Count-1) do
					(
						phEv = [ph.x+50, ph.y+50+(i*15)]
						rect = (box2 (phEV.x) (phEV.y) (maxlength+8) (14))
						
						gw.hrect rect aColor
						gw.htext [phEV.x+2,phEV.y,0] (_tags.Item[i]) color:black
					)
				) 
				gw.enlargeUpdateRect #whole
				gw.updateScreen()
			)
			registerRedrawViewsCallback showObjectNames
			redrawViews()
		)
		else
		(
			unRegisterRedrawViewsCallback showObjectNames
			redrawViews()
		)
	)
	
	toggleTagInfo()
) 