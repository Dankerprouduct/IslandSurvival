

objectiveX = nil;
objectiveY = nil;

pointIndex = 0; 
commandIndex = 0; 

proccessingCommand = false; 

function Update(  )
	
	if(proccessingCommand == false) then 
	RefreshTasks(); 
	task = GetTask(); 
	ProcessTaskA(task); 
	ProcessTaskC(); 

	end
	-- movement 
	positionX = GetPosition().X;
	positionY = GetPosition().Y;

	distX = GetNavigation(objectiveX, objectiveY, pointIndex).X; 
	distY = GetNavigation(objectiveX, objectiveY, pointIndex).Y; 
	distance = Distance(distX, distY);
	
	if(distance > 5.5) then 
	MoveTo(GetNavigation(objectiveX, objectiveY, pointIndex).X, GetNavigation(objectiveX, objectiveY, pointIndex).Y, (5 / distance));
	end
	if(distance < 5.5) then 
	NextPoint(); 
	end
end

function  ProcessTaskA( task )
	
	-- 4 is inventory length
	for i = 0, 4, +1
	do 
		if(task.job.PreferedObject == GetItem(i).name) then 
			
			ProcessTaskB(task);
		end
		if(task.job.ObjectType == GetItem(i).name) then 
			ProcessTaskB(task); 
		end
		
	end

end

function ProcessTaskB( task )
	
	if(task.job.name == "Hauler") then 
	
	AddCommand("Move", task.location1.x, task.location1.y); 
	AddCommand("Pickup", task.location1.x, task.location1.y); 
	AddCommand("Move", task.location2.x, task.location2.y); 
	AddCommand("Drop", task.location2.x, task.location2.y); 

	elseif(task.job.name == "Miner") then 

	AddCommand("Move", task.location1.x, task.location1.y); 
	AddCommand("Destroy", task.location1.x, task.location1.y); 
	AddCommand("Pickup", tasl.location1.x, task.location1.y); 

	elseif(task.job.name == "Forestry") then 

	AddCommand("Move", task.location1.x, task.location1.y); 
	AddCommand("Destroy", task.location1.x, task.location1.y);
	AddCommand("Pickup", task.location1.x, task.location1.y); 

	elseif(task.job.name == "Builder") then 

	AddCommand("Move", task.location1.x, task.location.y); 
	AddCommand("Build", task.location1.x, task.location.y, task.id);  

	elseif(task.job.name == "Hunter") then 

	-- needs a locate command

	elseif(task.job.name == "Nothing") then

	AddCommand("Move", positionX /32, positionY/ 32 ); 

	end

end

function ProcessTaskC()
	
	command = GetCommand(commandIndex); 

	if(command.type == Command.CommandType.Move) then 

	objectiveX = command.location.X; 
	objectiveY = command.location.Y; 
	NavigateTo(command.location.X, command.location.Y);

	elseif(command.type == Command.CommandType.Pickup) then 

	Pickup(command.location.X, command.location.Y); 

	elseif(command.type == Command.CommandType.Build) then 
	
	Build(command.location.X, command.location.Y); 

	elseif(command.type == Command.CommandType.Destroy) then 

	Destroy(command.location.X, command.location.Y); 

	elseif(command.type == Command.CommandType.Drop) then

	Drop(command.location.X, command.location.Y); 

	elseif(command.type == command.CommandType.Kill) then 

	Kill(command.index); 

	end

	RemoveCommand(commandIndex); 

end


function NextPoint( )
	if(pointIndex < GetPointNum()) then
	pointIndex = pointIndex + 1;
	proccessingCommand = true;
	elseif
	proccessingCommand = false; 
	end
end








