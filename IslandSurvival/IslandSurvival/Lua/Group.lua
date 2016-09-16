

print("made it this far"); 

minWood = 50; 
minStone = 50; 
cWood = 0; 
cStone = 0; 

-- TaskTypes
-- 1 - wood 
-- 2 - stone

taskAdded = false; 
function Update()
	
	cWood = GetWood(); 
	cStone = GetWood(); 


	-- how to keep it from spamming new task?
	if((cWood < minWood) and (taskAdded == false)) then 
	
	AddWoodTask(); 
	
	taskAdded = true;

	end 

	
	
end

function ResetTask()
	taskAdded = false; 
end 

function AddWoodTask() 
	GenerateNewTask(1);
end

function AddStoneTask()
	GenerateNewTask(2); 
end