# Use Cases Namespace

This is the 'Manager' of the application.
It communicates with other modules by providing policies and contracts which modules have to fullfil.
Like a business organization hierachy. 

In implementation that means Use Cases provides interface for modules to implement, together with data types.
Use Cases is alowed to communcate with all other modules, but all other modules communicate only with Use Cases, 
CEO.