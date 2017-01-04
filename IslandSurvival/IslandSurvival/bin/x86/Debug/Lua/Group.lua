
minWood = 50; 
minStone = 50; 
cWood = 0; 
cStone = 0; 

-- TaskTypes
-- 1 - wood 
-- 2 - stone

taskAdded = false; 

CollectWood = false; 
ColelctStone = false; 

function Update()
	
	cWood = GetWood(); 
	cStone = GetWood(); 

	if(cWood <= minWood) then 

	AddWoodTask(); 
	
	end

	-- how to keep it from spamming new task?
	if((cWood < minWood)) then 
	
	AddWoodTask(); 
	
	taskAdded = true;

	end 

	if((minWood > cWood)) then

	CollectWood = true; 

	elseif (cWood > minWood) then 

	CollectWood = false; 

	end

	
	
end

function ResetTask()
	taskAdded = false; 
end 

function AddWoodTask() 
	GenerateNewTask("Forestry");
end

function AddStoneTask()
	GenerateNewTask("Miner"); 
end