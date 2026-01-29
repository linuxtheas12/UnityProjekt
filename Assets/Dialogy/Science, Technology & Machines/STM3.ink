-> main
VAR correct = "To blow a fuse"
VAR chosen = ""

=== main ===
Mythical Wise Tree : Which idiom means to suddenly become very angry?
    + Well-oiled machine
        ~ chosen = "Well-oiled machine"
        #WRONG_CHOICE
        -> check_choice
    + On the same wavelength
        ~ chosen = "On the same wavelength"
        #WRONG_CHOICE
        -> check_choice
    + To blow a fuse
        ~ chosen = "To blow a fuse"
        #CORRECT_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Calm down! But yes, you got it right! | Incorrect. Think about an electrical surge!}
-> END