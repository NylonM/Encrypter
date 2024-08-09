from qiskit import  QuantumCircuit, transpile
from qiskit.providers.basic_provider import BasicProvider
from qiskit_ibm_runtime import EstimatorV2 as Estimator
from qiskit_ibm_runtime import SamplerV2 as Sampler,Session

from qiskit.primitives import BackendSampler
import os
import numpy as np
import firebase_admin
from firebase_admin import credentials , firestore, initialize_app , storage

cred = credentials.Certificate(r"c:/Users/osman/AppData/Local/Programs/Python/Python312/Lib/site-packages/qiskit-encryption-firebase-adminsdk-ljrth-806c0cb7d9.json")
try:
    firebase_admin.get_app()
except ValueError:
    firebase_admin.initialize_app(cred)

db = firestore.client()

qc = QuantumCircuit(5, 5)

qc.h(0)
qc.h(1)
qc.h(2)
qc.h(3)
qc.h(4)
qc.measure([0, 1, 2, 3, 4], [0, 1, 2, 3, 4]) 

backend_sim = BasicProvider().get_backend('basic_simulator')
#qasm_simulator changed with basic_simulator
sampler = BackendSampler(backend_sim)
#job_sim= transpile(circuit, backend_sim, shots =1024)
circuit = transpile(qc, backend_sim)
job_sim = sampler.run([qc])
result_sim = job_sim.result()
data_pub = result_sim.quasi_dists[0]
_str = ""
for i in data_pub:
    _str += str(i)+ " " 

data = {
    'key': _str
}
doc_ref = db.collection('quasi_dists').document()
doc_ref.set(data)
print(f"VariableValue:{data}")
def getQuasi():
    return data

def getParameter():
    return doc_ref.id


