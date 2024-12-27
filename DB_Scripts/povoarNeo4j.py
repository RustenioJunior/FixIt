from neo4j import GraphDatabase
from faker import Faker
from datetime import date
import random
import time

# Configuração do Neo4j
NEO4J_URI = "bolt://localhost:7687"
NEO4J_USER = "neo4j"
NEO4J_PASSWORD = "12345678"  # Substitua pelo password do seu banco

# Conexão com o banco Neo4j
driver = GraphDatabase.driver(NEO4J_URI, auth=(NEO4J_USER, NEO4J_PASSWORD))

# Inicializando o Faker para dados fictícios
faker = Faker()

# Função para criar registros no Neo4j
def criar_company(tx, name, nif, address, email, phone, created_date, modified_date, location_reference, postal_code):
    company_id = faker.uuid4()
    tx.run(
        """
        CREATE (n:Company {
            id: $id,
            name: $name,
            nif: $nif,
            address: $address,
            email: $email,
            phone: $phone,
            created_date: $created_date,
            modified_date: $modified_date,
            location_reference: $location_reference,
            postal_code: $postal_code
        })
        """,
        id=company_id, name=name, nif=nif, address=address, email=email, phone=phone,
        created_date=created_date, modified_date=modified_date,
        location_reference=location_reference, postal_code=postal_code
    )

def criar_role(tx, permissions, description):
    role_id = faker.uuid4()
    tx.run(
        """
        CREATE (n:Role {
            id: $id,
            permissions: $permissions,
            description: $description
        })
        """,
        id=role_id, permissions=permissions, description=description
    )

def criar_client(tx, name, role_id, company_id, verified, active, created_date, modified_date):
    client_id = faker.uuid4()
    tx.run("""
        MATCH (c:Role {id: $role_id}), (co:Company {id: $company_id})
        CREATE (n:Client {
            id: $id,
            name: $name, 
            verified: $verified, 
            active: $active, 
            created_date: $created_date, 
            modified_date: $modified_date
        })
        CREATE (n)-[:HAS_ROLE]->(c)
        CREATE (n)-[:HAS_COMPANY]->(co)
        """,
        id=client_id, name=name, role_id=role_id, company_id=company_id, verified=verified,
        active=active, created_date=created_date, modified_date=modified_date)

def create_machine_type(tx, description, active, created_date, modified_date):
    machine_type_id = faker.uuid4()
    tx.run("""
        CREATE (n:MachineType {
            id: $id,
            description: $description, 
            active: $active, 
            created_date: $created_date, 
            modified_date: $modified_date
        })
        """,
        id=machine_type_id, description=description, active=active, created_date=created_date, modified_date=modified_date)

def create_machine_model(tx, model, machine_type_id, created_date, modified_date):
    machine_model_id = faker.uuid4()
    tx.run("""
        MATCH (mt:MachineType {id: $machine_type_id})
        CREATE (n:MachineModel {
            id: $id,
            model: $model, 
            machine_type_id: $machine_type_id, 
            created_date: $created_date, 
            modified_date: $modified_date
        })
        CREATE (n)-[:IS_TYPE_OF]->(mt)
        """,
        id=machine_model_id, model=model, machine_type_id=machine_type_id, created_date=created_date, modified_date=modified_date)

def create_machine(tx, serial_number, machine_model_id, company_id, active, created_date, modified_date):
    machine_id = faker.uuid4()
    tx.run("""
        MATCH (mm:MachineModel {id: $machine_model_id}), (co:Company {id: $company_id})
        CREATE (n:Machine {
            id: $id,
            serial_number: $serial_number, 
            machine_model_id: $machine_model_id, 
            company_id: $company_id,
            active: $active,
            created_date: $created_date, 
            modified_date: $modified_date
        })
        CREATE (n)-[:IS_MODEL_OF]->(mm)
        CREATE (n)-[:HAS_COMPANY]->(co)
        """,
        id=machine_id, serial_number=serial_number, machine_model_id=machine_model_id, company_id=company_id, active=active, created_date=created_date, modified_date=modified_date)

def create_status(tx, description, created_date, modified_date):
    status_id = faker.uuid4()
    tx.run("""
        CREATE (n:Status {
            id: $id,
            description: $description, 
            created_date: $created_date, 
            modified_date: $modified_date
        })
        """,
        id=status_id, description=description, created_date=created_date, modified_date=modified_date)
    
def create_appointment(tx, client_id, machine_id, status_id, date_appointment, date_conclusion, created_date, modified_date):
    appointment_id = faker.uuid4()
    tx.run("""
        MATCH (c:Client {id: $client_id}), (m:Machine {id: $machine_id}), (s:Status {id: $status_id})
        CREATE (n:Appointment {
            id: $id,
            client_id: $client_id, 
            machine_id: $machine_id, 
            status_id: $status_id,
            date_appointment: $date_appointment,
            date_conclusion: $date_conclusion,
            created_date: $created_date, 
            modified_date: $modified_date
        })
        CREATE (n)-[:HAS_CLIENT]->(c)
        CREATE (n)-[:HAS_MACHINE]->(m)
        CREATE (n)-[:HAS_STATUS]->(s)
        """,
        id=appointment_id, client_id=client_id, machine_id=machine_id, status_id=status_id, date_appointment=date_appointment, date_conclusion=date_conclusion, created_date=created_date, modified_date=modified_date) 

def create_worker(tx, name, role_id, admission_date, verified, active, created_date, modified_date):
    worker_id = faker.uuid4()
    tx.run("""
        MATCH (r:Role {id: $role_id})
        CREATE (n:Worker {
            id: $id,
            name: $name, 
            role_id: $role_id, 
            admission_date: $admission_date, 
            verified: $verified,
            active: $active,
            created_date: $created_date, 
            modified_date: $modified_date
        })
        CREATE (n)-[:HAS_ROLE]->(r)
        """,
        id=worker_id, name=name, role_id=role_id, admission_date=admission_date, verified=verified, active=active, created_date=created_date, modified_date=modified_date)

def create_part(tx, name, description):
    part_id = faker.uuid4()
    tx.run("""
        CREATE (n:Part {
            id: $id,
            name: $name, 
            description: $description
        })
        """,
        id=part_id, name=name, description=description)

def create_service(tx, appointment_id, worker_id, part_id, dare_begin, date_conclusion, motive_reschedule, client_signature, created_date, modified_date):
    service_id = faker.uuid4()
    tx.run("""
        CREATE (n:Service {
            id: $id,
            appointment_id: $appointment_id, 
            worker_id: $worker_id, 
            part_id: $part_id, 
            date_begin: $dare_begin, 
            date_conclusion: $date_conclusion, 
            motive_reschedule: $motive_reschedule, 
            client_signature: $client_signature, 
            created_date: $created_date, 
            modified_date: $modified_date
        })
        CREATE (n)-[:HAS_APPOINTMENT]->(a:Appointment {id: $appointment_id})
        CREATE (n)-[:HAS_WORKER]->(w:Worker {id: $worker_id})
        CREATE (n)-[:HAS_PART]->(p:Part {id: $part_id})
        """,
        id=service_id, appointment_id=appointment_id, worker_id=worker_id, part_id=part_id, dare_begin=dare_begin, date_conclusion=date_conclusion, motive_reschedule=motive_reschedule, client_signature=client_signature, created_date=created_date, modified_date=modified_date)

def create_review(tx, service_id, client_id, review_star, comment, created_date, modified_date):
    review_id = faker.uuid4()
    tx.run("""
        CREATE (n:Review {
            id: $id,
            service_id: $service_id, 
            client_id: $client_id, 
            review_star: $review_star, 
            comment: $comment, 
            created_date: $created_date, 
            modified_date: $modified_date
        })
        CREATE (n)-[:HAS_SERVICE]->(s:Service {id: $service_id})
        CREATE (n)-[:HAS_CLIENT]->(c:Client {id: $client_id})
        """,
        id=review_id, service_id=service_id, client_id=client_id, review_star=review_star, comment=comment, created_date=created_date, modified_date=modified_date)

# Função principal para popular dados
start_time = time.time()
with driver.session() as session:
    # Create companies
    for _ in range(1000):
        name = faker.name()
        nif = faker.random_number(digits=9, fix_len=True)
        address = faker.address()
        email = faker.email()
        phone = faker.phone_number()
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        location_reference = faker.address()
        postal_code = faker.postcode()
        session.execute_write(criar_company, name, nif, address, email, phone, created_date, modified_date, location_reference, postal_code)

    # Create roles
    for _ in range(3):
        permissions = faker.word()
        description = faker.sentence()
        session.execute_write(criar_role, permissions, description)

    # Create clients
    roles = session.run("MATCH (r:Role) RETURN r.id AS id").data()
    companies = session.run("MATCH (c:Company) RETURN c.id AS id").data()

    if roles and companies:
        role_id = random.choice(roles)["id"]
        company_id = random.choice(companies)["id"]
    else:
        print("Roles ou Companies não existem no banco.")

    for _ in range(3000):
        name = faker.name()
        role_id = random.choice(roles)["id"]  # Extraindo apenas o valor do ID
        company_id = random.choice(companies)["id"]  # Extraindo apenas o valor do ID
        verified = random.randint(0, 1)
        active = random.randint(0, 1)
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(criar_client, name, role_id, company_id, verified, active, created_date, modified_date)
    
    # Create machine types
    for _ in range(100):
        description = faker.sentence()
        active = random.randint(0, 1)
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(create_machine_type, description, active, created_date, modified_date)
        
    # Create machine models
    machine_types = session.run("MATCH (mt:MachineType) RETURN mt.id AS id").data()
    if machine_types:
        machine_type_id = random.choice(machine_types)["id"]
    else:
        print("Machine Types não existem no banco.")

    for _ in range(200):
        model = faker.word()
        machine_type_id = random.choice(machine_types)["id"]  # Extraindo apenas o valor do ID
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(create_machine_model, model, machine_type_id, created_date, modified_date)
        
    # Create machines
    machine_models = session.run("MATCH (mm:MachineModel) RETURN mm.id AS id").data()
    if machine_models:
        machine_model_id = random.choice(machine_models)["id"]
    else:
        print("Machine Models não existem no banco.")

    companies = session.run("MATCH (c:Company) RETURN c.id AS id").data()
    if companies:
        company_id = random.choice(companies)["id"]
    else:
        print("Companies não existem no banco.")

    for _ in range(20000):
        serial_number = faker.random_number(digits=9, fix_len=True)
        machine_model_id = random.choice(machine_models)["id"]  # Extraindo apenas o valor do ID
        company_id = random.choice(companies)["id"]  # Extraindo apenas o valor do ID
        active = random.randint(0, 1)
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(create_machine, serial_number, machine_model_id, company_id, active, created_date, modified_date)   
    
    # Create statuses    
    for _ in range(5):
        description = random.choice(["Em andamento", "Concluído", "Cancelado", "Aberto", "Adiado"])
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(create_status, description, created_date, modified_date)   
        
    # Create appointments
    clients = session.run("MATCH (c:Client) RETURN c.id AS id").data()
    machines = session.run("MATCH (m:Machine) RETURN m.id AS id").data()
    statuses = session.run("MATCH (s:Status) RETURN s.id AS id").data()

    if clients and machines and statuses:
        client_id = random.choice(clients)["id"]
        machine_id = random.choice(machines)["id"]
        status_id = random.choice(statuses)["id"]
    else:
        print("Clients ou Machines ou Statuses não existem no banco.")

    for _ in range(200000):
        client_id = random.choice(clients)["id"]  # Extraindo apenas o valor do ID
        machine_id = random.choice(machines)["id"]  # Extraindo apenas o valor do ID
        status_id = random.choice(statuses)["id"]  # Extraindo apenas o valor do ID
        date_appointment = faker.date_between(start_date=date(2010, 1, 1), end_date=date(2023, 12, 31))
        date_conclusion = faker.date_between(start_date=date(2010, 1, 1), end_date=date(2023, 12, 31))
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(create_appointment, client_id, machine_id, status_id, date_appointment, date_conclusion, created_date, modified_date)    
    
    # create workers
    roles = session.run("MATCH (r:Role) RETURN r.id AS id").data()

    if roles and companies:
        role_id = random.choice(roles)["id"]
    else:
        print("Roles não existem no banco.")

    for _ in range(1000):
        name = faker.name()
        role_id = random.choice(roles)["id"]  # Extraindo apenas o valor do ID
        admission_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        verified = random.randint(0, 1)
        active = random.randint(0, 1)
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(create_worker, name, role_id, admission_date, verified, active, created_date, modified_date)
        
    # create parts
    for _ in range(100):
        name = faker.name()
        description = faker.sentence()
        session.execute_write(create_part, name, description)   
    
    # create services
    appointments = session.run("MATCH (a:Appointment) RETURN a.id AS id").data()
    workers = session.run("MATCH (w:Worker) RETURN w.id AS id").data()
    parts = session.run("MATCH (p:Part) RETURN p.id AS id").data()
    if appointments and workers and parts:
        appointment_id = random.choice(appointments)["id"]
        worker_id = random.choice(workers)["id"]
        part_id = random.choice(parts)["id"]
    else:
        print("Appointments ou Workers ou Parts não existem no banco.")
    for _ in range(150000):
        appointment_id = random.choice(appointments)["id"]  # Extraindo apenas o valor do ID
        worker_id = random.choice(workers)["id"]  # Extraindo apenas o valor do ID
        part_id = random.choice(parts)["id"]  # Extraindo apenas o valor do ID
        date_begin = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        date_conclusion = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        motive_reschedule = faker.sentence()
        client_signature = faker.name()
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(create_service, appointment_id, worker_id, part_id, date_begin, date_conclusion, motive_reschedule, client_signature, created_date, modified_date)     
        
        
    # create reviews
    services = session.run("MATCH (s:Service) RETURN s.id AS id").data()
    clients = session.run("MATCH (c:Client) RETURN c.id AS id").data()
    if services and clients:
        service_id = random.choice(services)["id"]
        client_id = random.choice(clients)["id"]
    else:
        print("Services ou Clients não existem no banco.")
    for _ in range(10000):
        service_id = random.choice(services)["id"]  # Extraindo apenas o valor do ID
        client_id = random.choice(clients)["id"]  # Extraindo apenas o valor do ID
        review_star = random.randint(1, 5)
        comment = faker.sentence()
        created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
        modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
        session.execute_write(create_review, service_id, client_id, review_star, comment, created_date, modified_date)                
end_time = time.time()
print(f"Dados populados em {end_time - start_time:.2f} segundos")
