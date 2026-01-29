-> main
VAR correct = "Light years ahead"
VAR chosen = ""

=== main ===
Jimmy the Joker : How do you describe something that is much further advanced?
    + On the same wavelength
        ~ chosen = "On the same wavelength"
          #WRONG_CHOICE
        -> check_choice
        
    + Light years ahead
        ~ chosen = "Light years ahead"
        #CORRECT_CHOICE
        -> check_choice
        
    + Cutting edge
        ~ chosen = "Cutting edge"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: You are a genius from the future! | No, that's too slow. Think about space travel!}
-> END