
objectiveX = nil;
objectiveY = nil;

pointIndex = 0; 
commandIndex = 0; 

proccessingCommand = false; 

function Update()
	
	if(GetCommandNum() == 0) then
		if(proccessingCommand == false) then 
		RefreshTasks(); 
		end
		if(proccessingCommand == false) then 
		task = GetTask();
		end
		if(proccessingCommand == false) then 
		ProcessTaskA(task);
		end

	end
	
	if(proccessingCommand == false) then 
	ProcessTaskC(); 
	end

	

	-- movement 
	positionX = GetPosition().X;
	positionY = GetPosition().Y;
	
	if(GetPointNum() > 0) then 
	distX = GetNavigation(objectiveX, objectiveY, pointIndex).X; 
	distY = GetNavigation(objectiveX, objectiveY, pointIndex).Y;
	distance = Distance(distX, distY);
	
	if(distance > 5.5) then 
	MoveTo(GetNavigation(objectiveX, objectiveY, pointIndex).X, GetNavigation(objectiveX, objectiveY, pointIndex).Y, (5 / distance));
	end
	if(distance < 5.5) then 
	print("next point"); 
	NextPoint(); 
	
	--RemovePoint(0); 
	end
	end
end

function  ProcessTaskA(task)
	
	-- 4 is inventory length
		if(InventoryContains(task.job.PreferedObject)) then  
			ProcessTaskB(task);
		elseif(InventoryContains(task.job.ObjectType)) then 
			ProcessTaskB(task); 
		end

end

function ProcessTaskB(task)
	
	if(proccessingCommand == false) then 
		if(task.job.name == "Hauler") then 
		AddCommand("Move", task.location1.X, task.location1.Y); 
		AddCommand("Pickup", task.location1.X, task.location1.Y); 
		AddCommand("Move", task.location2.X, task.location2.Y); 
		AddCommand("Drop", task.location2.X, task.location2.Y); 

		elseif(task.job.name == "Miner") then 
		print("adding Hauler task")
		AddCommand("Move", task.location1.X, task.location1.Y); 
		AddCommand("Destroy", task.location1.X, task.location1.Y); 
		AddCommand("Pickup", tasl.location1.X, task.location1.Y); 

		elseif(task.job.name == "Forestry") then 
		AddCommand("Move", task.location1.X, task.location1.Y); 
		AddCommand("Destroy", task.location1.X, task.location1.Y);
		AddCommand("Pickup", task.location1.X, task.location1.Y); 

		elseif(task.job.name == "Builder") then 
		AddCommand("Move", task.location1.X, task.location.Y); 
		AddCommand("Build", task.location1.X, task.location.Y, task.id);  

		elseif(task.job.name == "Hunter") then 
		-- needs a locate command

		elseif(task.job.name == "FreeTime") then
		print("nothing");
		AddCommand("Move", GetPosition().X /32, GetPosition().Y / 32 ); 

		end
	end
end

function ProcessTaskC()
	
	command = GetCommand(commandIndex); 
	if(GetCommandEnum(command.type) == 0) then -- Move

	objectiveX = command.location.X; 
	objectiveY = command.location.Y; 
	NavigateTo(command.location.X, command.location.Y);
	proccessingCommand = true;
	elseif(GetCommandEnum(command.type) == 1) then -- Pickup

	Pickup(command.location.X, command.location.Y); 

	elseif(GetCommandEnum(command.type) == 2) then -- Build
	
	Build(command.location.X, command.location.Y); 

	elseif(GetCommandEnum(command.type) == 3) then -- Destroy

	Destroy(command.location.X, command.location.Y); 

	elseif(GetCommandEnum(command.type) == 4) then -- Drop

	Drop(command.location.X, command.location.Y); 

	elseif(GetCommandEnum(command.type) == 5) then -- Kill

	Kill(command.index); 

	end
	
	RemoveCommand(commandIndex); 
	
	print("removed command" , command.type, proccessingCommand); 
end


function NextPoint()
	
	print(GetPointNum(),  proccessingCommand); 
	if(GetPointNum() > 0) then
		RemovePoint(0); 
	end
	print(GetPointNum()); 
	if(GetPointNum() == 0) then
	proccessingCommand = false; 
	end

end








