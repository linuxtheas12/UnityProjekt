-> main
VAR correct = "On the same wavelength"
VAR chosen = ""

=== main ===
Mythical Wise Tree : Which idiom means that two people understand each other perfectly?
    + To blow a fuse
        ~ chosen = "To blow a fuse"
        #WRONG_CHOICE
        -> check_choice
    + On the same wavelength
        ~ chosen = "On the same wavelength"
        #CORRECT_CHOICE
        -> check_choice
    + To pull the plug
        ~ chosen = "To pull the plug"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Exactly! We are definitely on the same wavelength! | No, that's not it. It's like a radio frequency!}
-> END