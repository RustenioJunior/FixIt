from pymongo import MongoClient
from faker import Faker
from datetime import date
import random
import time

start_time = time.time() 

# Configuração do MongoDB
MONGO_URI = "mongodb://localhost:27017/"
DATABASE_NAME = "industrial_maintenance"

client = MongoClient(MONGO_URI)
db = client[DATABASE_NAME]

# Instância do Faker
faker = Faker()

# Coleções
company_collection = db["company"]
role_collection = db["role"]
client_collection = db["client"]
machine_type_collection = db["machine_type"]
machine_model_collection = db["machine_model"]
machine_collection = db["machine"]
status_collection = db["status"]
appointment_collection = db["appointment"]
worker_collection = db["worker"]
parts_collection = db["parts"]
service_collection = db["service"]
review_collection = db["review"]

# Quantidades
define_quantities = {
    "companies": 1000,
    "clients": 3000,
    "roles": 3,
    "machine_types": 100,
    "machine_models": 200,
    "machines": 20000,
    "statuses": 5,
    "appointments": 200000,
    "workers": 1000,
    "parts": 100,
    "services": 150000,
    "reviews": 10000,
}

# Dados fictícios
company_ids = []
role_ids = []
client_ids = []
machine_type_ids = []
machine_model_ids = []
machine_ids = []
status_ids = []
appointment_ids = []
worker_ids = []
parts_ids = []
service_ids = []

# Inserção de registros
# Empresas
for _ in range(define_quantities["companies"]):
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    company = {
        "name": faker.company(),
        "nif": faker.random_number(digits=9, fix_len=True),
        "address": faker.address(),
        "email": faker.email(),
        "phone": faker.phone_number(),
        "created_date": str(created_date),
        "modified_date": str(modified_date),
        "location_reference": faker.address(),
        "postal_code": faker.postcode(),
    }
    company_ids.append(company_collection.insert_one(company).inserted_id)

# Funções
for _ in range(define_quantities["roles"]):
    role = {
        "permissions": faker.word(),
        "description": faker.sentence(),
    }
    role_ids.append(role_collection.insert_one(role).inserted_id)

# Clientes
for _ in range(define_quantities["clients"]):
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    client = {
        "name": faker.name(),
        "role_id": random.choice(role_ids),
        "company_id": random.choice(company_ids),
        "verified": random.randint(0, 1),
        "active": random.randint(0, 1),
        "created_date": str(created_date),
        "modified_date": str(modified_date),
    }
    client_ids.append(client_collection.insert_one(client).inserted_id)

# Tipos de Máquina
for _ in range(define_quantities["machine_types"]):
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    machine_type = {
        "description": faker.sentence(),
        "active": random.randint(0, 1),
        "created_date": str(created_date),
        "modified_date": str(modified_date),
    }
    machine_type_ids.append(machine_type_collection.insert_one(machine_type).inserted_id)

# Modelos de Máquina
for _ in range(define_quantities["machine_models"]):
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    machine_model = {
        "model": faker.word(),
        "machine_type_id": random.choice(machine_type_ids),
        "created_date": str(created_date),
        "modified_date": str(modified_date),
    }
    machine_model_ids.append(machine_model_collection.insert_one(machine_model).inserted_id)

# Máquinas
for _ in range(define_quantities["machines"]):
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    machine = {
        "serial_number": faker.random_number(digits=9, fix_len=True),
        "machine_model_id": random.choice(machine_model_ids),
        "company_id": random.choice(company_ids),
        "active": random.randint(0, 1),
        "created_date": str(created_date),
        "modified_date": str(modified_date),
    }
    machine_ids.append(machine_collection.insert_one(machine).inserted_id)

# Status
for _ in range(define_quantities["statuses"]):
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    status = {
        "description": random.choice(["Em andamento", "Concluído", "Cancelado", "Aberto", "Adiado"]),
        "created_date": str(created_date),
        "modified_date": str(modified_date),
    }
    status_ids.append(status_collection.insert_one(status).inserted_id)
    
# appointment
for i in range(define_quantities["appointments"]):
    date_appointment = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    date_conclusion = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    appointment = {
        "client_id": random.choice(client_ids),
        "machine_id": random.choice(machine_ids),
        "status_id": random.choice(status_ids),
        "date_appointment": str(date_appointment),
        "date_conclusion": str(date_conclusion),
        "created_date": str(created_date),
        "modified_date": str(modified_date),
    }   
    appointment_ids.append(appointment_collection.insert_one(appointment).inserted_id)
    
# worker
for i in range(define_quantities["workers"]):
    admission_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    verifed = random.randint(0, 1)
    active = random.randint(0, 1)
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    worker = {
        "name": faker.name(),  
        "admission_date": str(admission_date),
        "verified": verifed,
        "active": active,  
        "created_date": str(created_date),
        "modified_date": str(modified_date),
    } 
    worker_ids.append(worker_collection.insert_one(worker).inserted_id)

# parts
for i in range(define_quantities["parts"]):
    parts = {
        "name": faker.word(),
        "description": faker.word(),
    } 
    parts_ids.append(parts_collection.insert_one(parts).inserted_id) 
    
# services
for i in range(define_quantities["services"]):
    appointment_id  = random.choice(appointment_ids)
    worker_id = random.choice(worker_ids)
    part_id = random.choice(parts_ids)
    date_begin = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    date_conclusion = faker.date_between_dates(date_start=date_begin, date_end=date(2023, 12, 31))
    motive_reschedule = faker.word()
    client_signature = faker.name()
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    service = {
        "appointment_id": appointment_id,
        "worker_id": worker_id,
        "part_id": part_id,
        "date_begin": str(date_begin),
        "date_conclusion": str(date_conclusion),
        "motive_reschedule": motive_reschedule,
        "client_signature": client_signature,
        "created_date": str(created_date),
        "modified_date": str(modified_date),   
    }   
    
    service_ids.append(service_collection.insert_one(service).inserted_id)   
    
# reviewer
for i in range(define_quantities["reviews"]):
    service_id = random.choice(service_ids)
    client_id = random.choice(client_ids)
    review_star = random.randint(1, 5)
    comment = faker.sentence()
    created_date = faker.date_between_dates(date_start=date(2010, 1, 1), date_end=date(2023, 12, 31))
    modified_date = faker.date_between_dates(date_start=created_date, date_end=date(2023, 12, 31))
    review = {
        "service_id": service_id,
        "client_id": client_id,
        "review_star": review_star,
        "comment": comment,
        "created_date": str(created_date),
        "modified_date": str(modified_date),
    }       
    
end_time = time.time()  # Fim da medição do tempo
execution_time = end_time - start_time  # Calcula o tempo de execução
print(f"Tempo de execução do script original: {execution_time:.2f} segundos") 


# Agendamentos e outros dados podem ser adaptados da mesma forma, com as relações mantidas por IDs referenciados.
