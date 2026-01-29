-> main
VAR correct = "A well-oiled machine"
VAR chosen = ""

=== main ===
Savage Camel : How do you describe an organization or a team that works very smoothly?
    + Cutting edge
        ~ chosen = "Cutting edge"
        #WRONG_CHOICE
        -> check_choice
        
    + A well-oiled machine
        ~ chosen = "A well-oiled machine"
        #CORRECT_CHOICE
        -> check_choice
        
    + Cog in the machine
        ~ chosen = "Cog in the machine"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Yes! Smooth as butter! | Incorrect. Think about maintenance!}
-> END