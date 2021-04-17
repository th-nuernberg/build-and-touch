// dimensions of the device that should be enclosed (in mm)
h = 35;
width = 40;
length = 87;

thickness = 3; // thickness of the walls

//cutout
cWidth = 26;
cLength = 17;

// Wheter to provide a hole for the velcro-streps
strapholder = true;
sWidth = 13;
sHeight = 3;

// Calculated properties for later use
dthick = 2*thickness;

 height = strapholder ? h + sHeight : h;

// outer dimensions
oLength = length+dthick;
oWidth = width+dthick;
oHeight = height+dthick;

// Box
difference() {    
    cube([oLength,oWidth,oHeight]);       
    
    if(strapholder){
        translate(v=[dthick, - 0.5, thickness])  cube([sWidth, oWidth+1, sHeight]);
        translate([oLength-dthick-sWidth, - 0.5, thickness])  cube([sWidth, oWidth+1, sHeight]);        
    }
       
    translate(v=[thickness,thickness,thickness])  cube([length,width, oHeight]);
    translate(v=[length,dthick,dthick])  cube([dthick*2, width-dthick, height-dthick ]);        
}

// Lid
translate(v=[0, width+10, 0]){
    difference(){
            // Lid itself
        union() {
            cube([oLength,oWidth,thickness]);
            translate(v=[thickness, thickness, thickness]) cube([length, width, thickness]);            
        }
            //cutout
         translate(v=[0, width/2, thickness]) cube([cLength+ thickness, cWidth, dthick+1], center = true);             
    }      
}