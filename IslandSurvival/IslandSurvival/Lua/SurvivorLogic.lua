
-- notes -- 
-- have groups assign jobs like chop plants and cut to have someone pickup the logs. 
name = GetName(); 

positionX = 0; 
positionY = 0; 

objectiveX = GetPosition().X / 32; 
objectiveY = GetPosition().Y / 32 ; 


speed = math.random(1, 5); 

UpdateTasks(); 

task = GetTask();



objectiveX = task.location.x; 
objectiveY = task.location.y; 

if(objectiveX == 0) then 
		if(objectiveY == 0) then 
		NewObjective(); 

		end

	end 


print(name, ": objective", objectiveX , objectiveY); 

i = 0; 
aStar = {};

NavigateTo(objectiveX, objectiveY); 

working = false; 


function Update( )
	
	
	positionX = GetPosition().X;
	positionY = GetPosition().Y; 
	
	distX = GetNavigation(objectiveX, objectiveY, i).X;  
	distY = GetNavigation(objectiveX, objectiveY, i).Y;
	
	t = Distance(distX, distY); 

	if(t > .5 + speed) then 
	
	MoveTo(GetNavigation(objectiveX, objectiveY, i).X, GetNavigation(objectiveX, objectiveY, i).Y, (speed / t) ); 	
		
	end
	if(t < .5 + speed) then
	ChooseLocation();
	end

	--print(objectiveX, objectiveY, GetMapPosition().X, GetMapPosition().Y);

	-- Does 

	if(task.job.name == "Forestry") then 
	if( (GetMapPosition().X == objectiveX) and (GetMapPosition().Y == objectiveY) ) then 

	DamageMaterial(task.index, 100);
	NewObjective();
	
	elseif ( (GetMapPosition().X == objectiveX + 1) and (GetMapPosition().Y == objectiveY) ) then 

	DamageMaterial(task.index, 100);
	NewObjective();
	
	elseif ( (GetMapPosition().X == objectiveX - 1) and (GetMapPosition().Y == objectiveY) ) then

	DamageMaterial(task.index, 100);
	NewObjective();

	elseif( (GetMapPosition().X == objectiveX) and (GetMapPosition().Y == objectiveY + 1) ) then 

	DamageMaterial(task.index, 100);
	NewObjective();

	elseif ( (GetMapPosition().X == objectiveX) and (GetMapPosition().Y == objectiveY - 1) ) then 

	DamageMaterial(task.index, 100);
	NewObjective();

	elseif ( (GetMapPosition().X == objectiveX + 1) and (GetMapPosition().Y == objectiveY - 1)) then 

	DamageMaterial(task.index, 100);
	NewObjective();
	
	elseif ( (GetMapPosition().X == objectiveX + 1) and (GetMapPosition().Y == objectiveY + 1) ) then

	DamageMaterial(task.index, 100);
	NewObjective();

	elseif( (GetMapPosition().X == objectiveX - 1) and (GetMapPosition().Y == objectiveY + 1) ) then 

	DamageMaterial(task.index, 100);
	NewObjective();

	elseif ( (GetMapPosition().X == objectiveX - 1) and (GetMapPosition().Y == objectiveY - 1) ) then 

	DamageMaterial(task.index, 100);
	NewObjective();

	end 

	end

	if(task.job.name == "Hauler") then 

	if(IsNear() == true)
		AddToInventory(); 
	end

	end
	
end

function NewObjective()
	-- body
	UpdateTasks(); 
	task = GetTask();

	if(task.taskType ~= 0) then 
	objectiveX = task.location.x; 
	objectiveY = task.location.y; 

	print(name, ": objective is " , objectiveX, " " ,objectiveY); 
	
	i = 0;
	NavigateTo(objectiveX, objectiveY); 
	end
	if(task.taskType == 0) then 

	objectiveX = GetMapPosition().X;
	objectiveY = GetMapPosition().Y;  
	i = 0; 
	NavigateTo(objectiveX, objectiveY); 
	end

	end


function ChooseLocation()
	if(i < GetPoints()) then
	i = i + 1; 
	end
end

function IsNear(){
	if( (GetMapPosition().X == objectiveX) and (GetMapPosition().Y == objectiveY) ) then 

	return true; 
	
	elseif ( (GetMapPosition().X == objectiveX + 1) and (GetMapPosition().Y == objectiveY) ) then 

	return true; 
	
	elseif ( (GetMapPosition().X == objectiveX - 1) and (GetMapPosition().Y == objectiveY) ) then

	return true; 

	elseif( (GetMapPosition().X == objectiveX) and (GetMapPosition().Y == objectiveY + 1) ) then 

	return true; 

	elseif ( (GetMapPosition().X == objectiveX) and (GetMapPosition().Y == objectiveY - 1) ) then 

	return true; 

	elseif ( (GetMapPosition().X == objectiveX + 1) and (GetMapPosition().Y == objectiveY - 1)) then 

	return true; 
	
	elseif ( (GetMapPosition().X == objectiveX + 1) and (GetMapPosition().Y == objectiveY + 1) ) then

	return true; 

	elseif( (GetMapPosition().X == objectiveX - 1) and (GetMapPosition().Y == objectiveY + 1) ) then 

	return true; 

	elseif ( (GetMapPosition().X == objectiveX - 1) and (GetMapPosition().Y == objectiveY - 1) ) then 

	return true; 

	end

	return false;
}