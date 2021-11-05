
function StartTaskClick() {
    console.log("StartTaskClick");
    const textAreaElement = document.getElementById('data') as HTMLTextAreaElement;
    const iterationCountInputElement = document.getElementById('iterationCount') as HTMLInputElement;
    const dbProviderSelectElement = document.getElementById('dbProvider') as HTMLSelectElement;
    const operationSelectElement = document.getElementById('operation') as HTMLSelectElement;

    var task = {
        TaskIdentifier: this.GenerateIdentifier(),
        Data: textAreaElement.value,
        IterationCount: iterationCountInputElement.value,
        DatabaseProvider: dbProviderSelectElement.value,
        Operation: operationSelectElement.value
    };

    const response = fetch('DatabasePerformance', {
        method: 'post',
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify(task)
    }).then(response => {
        if (!response.ok) {
            throw response;
        };
    });
}

function GenerateIdentifier() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
}