positionX = 0; 
positionY = 0; 

objectiveX = GetPosition().X / 32; 
objectiveY = GetPosition().Y / 32 ; 

speed = math.random(0, 5); 


i = 0; 
aStar = {};

NavigateTo(objectiveX, objectiveY); 



function Update( )
	--body

	positionX = GetPosition().X;
	positionY = GetPosition().Y; 
	
	distX = GetNavigation(objectiveX, objectiveY, i).X; 
	distY = GetNavigation(objectiveX, objectiveY, i).Y;
	
	t = Distance(distX, distY); 
	if(t > .5 + speed) then 
	print(t);
	MoveTo(GetNavigation(objectiveX, objectiveY, i).X, GetNavigation(objectiveX, objectiveY, i).Y, (speed / t) ); 	

	end
	if(t < .5 + speed) then
	ChooseLocation();
	end
	
end

function ChooseLocation()
	if(i < GetPoints()) then
	i = i + 1; 
	end
end