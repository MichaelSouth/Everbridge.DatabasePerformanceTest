var mygrid;
function StartTaskClick() {
    console.log("StartTaskClick");
    const textAreaElement = document.getElementById('data');
    const iterationCountInputElement = document.getElementById('iterationCount');
    const dbProviderSelectElement = document.getElementById('dbProvider');
    const operationSelectElement = document.getElementById('operation');
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
        }
        ;
    });
    return;
}
function GenerateIdentifier() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
}
function renderGrid() {
    console.info('Render Grid');
    // @ts-ignore
    mygrid = new gridjs.Grid({
        columns: [{
                id: 'taskIdentifier',
                name: "Task",
                width: '20%'
            }, {
                id: 'databaseProvider',
                name: "Provider",
                width: '10%'
            }, {
                id: 'operation',
                name: "Operation",
                width: '10%'
            }, {
                id: 'iterationCount',
                name: "Iteration",
                width: '10%'
            }, {
                id: 'startTime',
                name: "Start Time",
                width: '10%',
                formatter: (cell) => {
                    const tempDate = new Date(cell);
                    const timeString = tempDate.toLocaleTimeString('en-US');
                    return `${timeString}`;
                }
            }, {
                id: 'endTime',
                name: "End Time",
                width: '10%',
                formatter: (cell) => {
                    const tempDate = new Date(cell);
                    const timeString = tempDate.toLocaleTimeString('en-US');
                    return `${timeString}`;
                }
            }, {
                id: 'executionTime',
                name: "Execution Time",
                width: '10%',
                formatter: (cell) => {
                    return `${cell}ms`;
                }
            }],
        sort: true,
        data: []
    }).render(document.getElementById("grid"));
}
function GetTasks() {
    //console.info('Fetching tasks');
    fetch('DatabasePerformance')
        //.then(CheckError)
        .then(response => response.json())
        .then(data => {
        // console.log(data);
        // @ts-ignore
        // Update Datas
        if (data.length > 0) {
            mygrid.updateConfig({
                data: data
            }).forceRender();
        }
        this.window.setTimeout(GetTasks, 1000);
    }).catch((error) => {
        // Handle the error
        console.log(error);
    });
}
renderGrid();
GetTasks();
//# sourceMappingURL=main.js.map