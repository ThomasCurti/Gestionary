# Gestionary

IIS + sql server Deploy
==

Prerequisite
--
Having SQL Server 2019 + visual studio 2019 + IIS installed  

TODO
--

### Creating the database

Create a new database on your machine using SQL Server 2019.  
(In SQL Server 2019: database -> right click -> new database).

1. Open the solution in Gestionary folder.

2. Right click on 'GestionaryDatabase' project and select 'Schema Comparisons'. On the left side of the Schema Comparisons should be the 'GestionaryDatabase' project.

3. On the right side, select the database you have created.

4. Compare the project and you database then chose 'Update' to import the project tables and views to your database.

5. The website need to have some data in the database in order to work properly. In your SQL Server, open the file in 'GestionaryDatabase\dbo\Datas\AddData.sql' and execute it in your 'Gestionary' database. It will create the necessary data in your database. It will add a admin user with name: admin, and password: admin, that you will be able to change in the website.

### Publish the 'GestionaryWebsite' project

1. In visual studio, right click the 'GestionaryWebsite' project and select 'Publish'.

2. In Publish, select 'FolderProfile' then chose the location of the folder. Usually website are deployed in the 'C:\inetpub\wwwroot\\' folder. So in our case the location can be 'C:\inetpub\wwwroot\GestionaryWebsite\\'.

3. Depending on your environment you will have to change the 'ConnectionString' to match your SQL Server instance and Database.  
You will have to replace the 'ConnectionString' by 'Data Source={Your instance};Initial Catalog={The name of your database};Trusted_Connection=True' in the settings of the publication.  
Example: "Data Source=.\MTI;Initial Catalog=Gestionary;Trusted_Connection=True"  

### IIS

##### Security

In order to increase the security of your application, you can create a new user on your machine with standards authorisations, add this user to the 'Gestionary' database in your SQL Server and create a new application pool in IIS that is taking the identity of the new user to access the database / website.

##### Deployment

Now that the website has been published and linked to the database, we can deploy the website with IIS.

1. In IIS, create a new website, select the name and the application pool you will use to manage the website.

2. In 'Select Physical path', enter the path where you published the Website with visual studio.

3. Chose the hostname and IP address / port as you wish.

4. Run the website, your website should now be deployed.

Docker Deploy
==

Prerequisite
--
Have docker and docker-compose installed

TODO
--

Open a powershell and type:

Powershell> docker-compose -f docker-compose.yml build

When the build is finished type:

Powershell> docker-compose -f docker-compose.yml up

/!\ Be careful, the exposed port is the port 5000, you can change it in the docker-compose.yml AND in the DockerFile inside the GestionaryWebsite folder

You can connect on the website at the address: localhost:5000
