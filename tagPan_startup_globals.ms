global tagForm
global callbackItem
global startAuto
maxpath=symbolicPaths.getPathValue"$max"
pathdll = maxpath+@"\plugins\"
global dll = (dotnetclass "System.Reflection.Assembly").Load ((dotnetclass "System.IO.File").ReadAllBytes (pathdll+"TagPan.dll"))
global treePan
global autoPan
global toggle = false
global showObjectNames
global PostNodeCloned
global ClonedPostProcess
global SaveEvent
global SaveEventC
global PreOpenEventC
global PostOpenEventC
global iniPath=(pathdll+"TagPan.ini")
global clonesHolder=#()
global debug