function deleteRow(btn) {
    var row = btn.parentNode.parentNode;
    row.parentNode.removeChild(row);
}
function addParam(tagID) {
    $(document).ready(function () {
        console.log(`${tagID}`);
        $(`#${tagID}`).append(`<tr><td><div class="form-group"><input class="form-control"></div></td><td><div class="form-group"><select class="form-control"><option>Select . . .</option><option>BadRequest</option><option>Unauthorized</option><option>NotFound</option><option>OK</option><option>InternalServerError</option></select></div></td><td><div class="form-group"><input type="checkbox" name="idRandom" /></div></td><td><div class="form-group"><input type="checkbox" name="idNull" /></div></td><td><div class="form-group"><input type="checkbox" name="idBlank" /></div></td><td><div class="form-group"><input class="form-control" /></div></td><td><div class="form-group"><input class="form-control" /></div></td><td><button type="button" class="btn btn-danger" onclick="deleteRow(this)">X</button></td></tr>
        `);
    });
}
function loadFileAsText() {

    var fileToLoad = document.getElementById("fileToLoad").files[0];

    var fileReader = new FileReader();
    fileReader.onload = function (fileLoadedEvent) {
        var textFromFileLoaded = fileLoadedEvent.target.result;

        document.getElementById("JSONTextArea").value = textFromFileLoaded;
    };
    fileReader.readAsText(fileToLoad, "UTF-8");
}

function JSONtoHTMLFields() {
    console.time('jsonLoadTime');
    var theJSON = document.getElementById("JSONTextArea").value;

    var printError = function (error, explicit) {
        var err = `[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`;
        $(document).ready(function () {
            //alert("butt");

            $("#jsonErrField").html(`<div class="alert alert-danger">"${err}"</div>`);
        });
    }

    try {
        var json = JSON.parse(theJSON);
        console.table(json);
        $(document).ready(function () {
            var htmlAPIDetails = "<div class=\"form-group\">" +
                "<label for=\"exampleFormControlSelect1\">Post Type</label>" +
                "<select class=\"form-control\" id=\"selectAPIAction\">" +
                "<option>Select API Action . . . </option>" +
                "<option>API POST</option>" +
                "<option>API PUT</option>" +
                "</select>" +
                "</div>";
            var htmlURIURL = "<div class=\"form-group\"><label for=\"exampleFormControlInput1\">URI/URL</label><input type=\"url\" class=\"form-control\" id=\"APIendpointURL\" placeholder=\"www.yourendpoint.com\"></div>";
            var htmlTableHeader = `<div class="form-group">
            <table class="table table-striped">
                <tr><th>Property Name</th><th>Data Type</th><th>Required</th><th>Default Value</th><th></th></tr>`;
            var htmlString = `<div class="panel panel-default"><div class="panel-heading">Object Details</div><form id="ComponentTestObjectData" action="post">${htmlAPIDetails}${htmlURIURL}<div class="scrollbar">${htmlTableHeader}<\d`;
            for (var key in json) {

                htmlString += `
                <tr>
                    <td>
                        <div class="form-group">
                            <label>${key}</label>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <select class="form-control" id="${key}DataType">
                                <option>Select Data Type</option>
                                <option>Boolean</option>
                                <option>DateTime</option>
                                <option>Decimel</option>
                                <option>Int</option>
                                <option>String</option>
                                <option>Complex</option>
                            </select>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <input type="checkbox" id="${key}Required">
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <input class="form-control" id="${key}DefaultValue" value="${json[key]}" width="40">
                        </div>
                    </td>
                    <td>
                        <button id="modalActivate" type="button" class="btn btn-danger" data-toggle="modal" data-target="#${key}Preferences">🖉 Edit Parameters</button>
                        <div class="modal fade right" id="${key}Preferences" tabindex="-1" role="dialog" aria-labelledby="exampleModalPreviewLabel" aria-hidden="true">
                            <div class="modal-dialog modal-lg" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalPreviewLabel">Attribute Preferences</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <div id="ParameterMenu">
                                            <div class="panel-body">
                                                <table id="${key}ParameterTable" class="table table-striped">
                                                    <tr><th>Expected Message</th><th>HTTP Response</th><th>Random</th><th>Null</th><th>Blank</th><th>Value/Char Length</th><th>Test Name</th><th><button type="button" class="btn btn-success" onclick="addParam('${key}ParameterTable')">+ Add Parameter</button></th></tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Done</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>`;
            }

            htmlString += `
                        </div>
                    </div>
                <div>
            </div>
            <div class="form-group">
                <label>DTO Name:</label>
                <input class="form-control">
            </div>
            <div class="panel-footer">
            <button class="btn btn-success\">&#9658; Run</button>
            </div>
        </div></div></form>`
            $("#mainGUI").html(htmlString);
            $("#jsonErrField").html("");
        });
    } catch (e) {
        if (e instanceof SyntaxError) {
            printError(e, true);
        }
        else {
            printError(e, false);
        }
    }
    console.timeEnd('jsonLoadTime');
}