-> main
VAR correct = "To pull the plug"
VAR chosen = ""

=== main ===
Savage Camel : Which idiom means to stop an activity or a project suddenly?
    + To pull the plug
        ~ chosen = "To pull the plug"
        #CORRECT_CHOICE
        -> check_choice
        
    + To blow a fuse
        ~ chosen = "To blow a fuse"
        #WRONG_CHOICE
        -> check_choice
        
    + On the same wavelength
        ~ chosen = "On the same wavelength"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Game over! You pulled the plug correctly! | Not quite. Think about disconnecting the power.}
-> END