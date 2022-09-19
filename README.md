# Asset Tracking 

This project is the start of an Asset Tracking database. It should have input possibilities from a user and print out functionality of user data.

Asset Tracking is a way to keep track of the company assets, like Laptops, Stationary computers, Phones and so on... 

All assets have an end of life which for simplicity reasons is 3 years. 

## Level 1

Create a console app that have classes and objects. For example like below with computers and phones.

Laptop Computers 
- MacBook
- Asus
- Lenovo

Mobile Phones 
- Iphone
- Samsung
- Nokia

You will need to create the appropriate properties and constructorsfor each object, like purchase date, price, model name etc. 

## Level 2

Create a program to create a list of assets (inputs) where the final result is to write the following to the console:

- Sorted list with Class as primary (computers first, then phones)
- Then sorted by purchase date
- Mark any item *RED* if purchase date is less than 3 months away from 3 years.

## Level 3 (<- CHOICE)

Add offices to the model: 

You should be able to place items in 3 different offices around the world which will use the appropriate currency for that country. You should be able to input values in dollars and convert them to each currency (based on todays currency charts)

When you write the list to the console: 
- [x] Sorted first by office
- [x] Then Purchase date
- [x] Items *RED* if date less than 3 months away from 3 years
- [x] Items *Yellow* if date less than 6 months away from 3 years
- [x] Each item should have currency according to country
