//debugger

//sets the natute of claim picklist value on the claim item form
function switchForms() 
{    	
	var cItems = getPickListValues();	
	var selectedFrm = Xrm.Page.ui.formSelector.getCurrentItem().getLabel();

	for(var j in cItems)
	{
		if(compare(selectedFrm , cItems[j].text) == true)
		{
			Xrm.Page.getAttribute("nv_nature_of_claim").setValue(cItems[j].value);
			Xrm.Page.data.entity.save();
			j = cItems.length+1;
		}		
	}	
	//var something = document.getElementById("crmFormSelector");
	//document.getElementById("crmFormSelector").style.display = "none";
}

//retrieves picklist values and labels (object) from crm
function getPickListValues()
{
	var pickListValues = Xrm.Page.getAttribute("nv_nature_of_claim").getOptions();
	return pickListValues;
}

//compares selected form label to pick list (nature of claim) 
function compare(formLabel, fieldLabel)
{
	if(formLabel == fieldLabel)
	{
		return true;
	}
	else
	{
		return false;
	}
}