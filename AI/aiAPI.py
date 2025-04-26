from flask import Flask, request, jsonify
from flask_cors import CORS
from transformers import AutoTokenizer, AutoModelForCausalLM

tokenizer = AutoTokenizer.from_pretrained("TinyLlama/TinyLlama-1.1B-Chat-v1.0")
model = AutoModelForCausalLM.from_pretrained(
    "TinyLlama/TinyLlama-1.1B-Chat-v1.0")

app = Flask(__name__)
CORS(app)


@app.route('/generate', methods=['POST'])
def generate():
    data = request.json
    prompt = data.get('prompt', '')
    formatted_prompt = f"Q: {prompt}\nA:"
    inputs = tokenizer(formatted_prompt, return_tensors="pt")
    outputs = model.generate(**inputs, max_new_tokens=128)
    response = tokenizer.decode(outputs[0], skip_special_tokens=True)
    # Extract only the answer after 'A:'
    answer = response.split("A:")[-1].strip()
    return jsonify({'response': answer})


if __name__ == '__main__':
    app.run(port=5000)
