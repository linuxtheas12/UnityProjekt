-> main
VAR correct = "To beat around the bush"
VAR chosen = ""

=== main ===
Mythical Wise Tree : Which idiom means to avoid talking about what is truly important?
    + To let the cat out of the bag
        ~ chosen = "To let the cat out of the bag"
        #WRONG_CHOICE
        -> check_choice
        
    + To be in the same boat
        ~ chosen = "To be in the same boat"
       #WRONG_CHOICE
        -> check_choice
        
    + To beat around the bush
        ~ chosen = "To beat around the bush"
        #CORRECT_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Stop hitting the plants and speak up! Correct! | No, you are missing the point. Think about garden shrubbery.}
-> END