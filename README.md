# LendingProject
In this project we have 2 services: Lender and Lending.
The Lender service responsible to get new customers that want to lend via our project, or to edit their rules.
The LendingService responsible to get request from peple that want to lend from specific lender, and send the request to an handler that check if the request is valid.

The big advantage of this system is, that the rules can be whatever the customer will write in the excel file he send. the Lender service read the excel and store the rules, that can be double, string or boolean expressions, and than every request will be testet according this rules. 
