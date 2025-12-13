-> main
VAR correct = "Marek"
VAR chosen = ""

=== main ===
Which pokemon do you choose?
    + Marek
        ~ chosen = "Marek"
        #CORRECT_CHOICE
        -> check_choice
        
    + Artur
        ~ chosen = "Artur"
        #WRONG_CHOICE
        -> check_choice
        
    + David
        ~ chosen = "David"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
You chose {chosen}{chosen == correct: , which is correct! | , which is wrong!}
-> END