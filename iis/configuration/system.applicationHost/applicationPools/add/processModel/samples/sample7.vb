Set adminManager = createObject("Microsoft.ApplicationHost.WritableAdminManager")
adminManager.CommitPath = "MACHINE/WEBROOT/APPHOST"
Set applicationPoolsSection = adminManager.GetAdminSection("system.applicationHost/applicationPools", "MACHINE/WEBROOT/APPHOST")
Set applicationPoolsCollection = applicationPoolsSection.Collection

addElementPos = FindElement(applicationPoolsCollection, "add", Array("name", "Contoso"))
If siteElementPos = -1 Then
   WScript.Echo "Element not found!"
   WScript.Quit
End If

Set addElement = applicationPoolsCollection.Item(addElementPos)
Set processModelElement = addElement.ChildElements.Item("processModel")
processModelElement.Properties.Item("shutdownTimeLimit").Value = "00:00:30"
processModelElement.Properties.Item("startupTimeLimit").Value = "00:00:30"

adminManager.CommitChanges()

Function FindElement(collection, elementTagName, valuesToMatch)
   For i = 0 To CInt(collection.Count) - 1
      Set element = collection.Item(i)
      If element.Name = elementTagName Then
         matches = True
         For iVal = 0 To UBound(valuesToMatch) Step 2
            Set property = element.GetPropertyByName(valuesToMatch(iVal))
            value = property.Value
            If Not IsNull(value) Then
               value = CStr(value)
            End If
            If Not value = CStr(valuesToMatch(iVal + 1)) Then
               matches = False
               Exit For
            End If
         Next
         If matches Then
            Exit For
         End If
      End If
   Next
   If matches Then
      FindElement = i
   Else
      FindElement = -1
   End If
End Function