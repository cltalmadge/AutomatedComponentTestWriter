



function loadFileAsText() {

    var fileToLoad = document.getElementById("fileToLoad").files[0];

    var fileReader = new FileReader();
    fileReader.onload = function (fileLoadedEvent) {
        var textFromFileLoaded = fileLoadedEvent.target.result;

        document.getElementById("JSONTextArea").value = textFromFileLoaded;
    };
    fileReader.readAsText(fileToLoad, "UTF-8");
}

//function JSONtoHTMLFields() {
//    console.time('jsonLoadTime');
//    var theJSON = document.getElementById("JSONTextArea").value;
//    var printError = function (error, explicit) {
//        var err = `[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`;
//        $(document).ready(function () {
//            //alert("butt");

//            $("#jsonErrField").html(`<div class="alert alert-danger">"${err}"</div>`);
//        });
//    }

//    try {
//        var json = JSON.parse(theJSON);
//        console.table(json);
//        $(document).ready(function () {
//            var htmlString = `<div class="panel panel-default">
//<div class="panel-heading">Object Details</div>
//        <form action="Yeet" method="POST">
//            <div class="panel-body">
//                <div class="form-group">
//                    <label for="exampleFormControlSelect1">Post Type</label>
//                    <select name="APIAction" class="form-control" id="selectAPIAction">
//                        <option>Select API Action . . . </option>
//                        <option>API POST</option>
//                        <option>API PUT</option>
//                    </select>
//                    <div class="form-group">
//                        <label for="exampleFormControlInput1">URI/URL</label>
//                        <input name="APIendpointURL" type="url" class="form-control" id="APIendpointURL" placeholder="www.yourendpoint.com">
//                    </div>
//                </div>
//                <div class="scrollbar">
//                    <table class="table table-striped">
//                        <tr><th>Property Name</th><th>Data Type</th><th>Required</th><th>Default Value</th><th></th></tr>`;
//            // For each attribute, iteratively concatenate template strings to table.
//            for (var key in json) {
//                htmlString += `
//                <tr>
//                    <td>
//                        <div class="form-group">
//                            <label name="${key}Label">${key}</label>
//                        </div>
//                    </td>
//                    <td>
//                        <div class="form-group">
//                            <select name="${key}DataType" class="form-control" id="${key}DataType">
//                                <option>Select Data Type</option>
//                                <option>Boolean</option>
//                                <option>DateTime</option>
//                                <option>Decimel</option>
//                                <option>Int</option>
//                                <option>String</option>
//                                <option>Complex</option>
//                            </select>
//                        </div>
//                    </td>
//                    <td>
//                        <div class="form-group">
//                            <input name="${key}Required" type="checkbox" id="${key}Required">
//                        </div>
//                    </td>
//                    <td>
//                        <div class="form-group">
//                            <input name="${key}DefaultValue" class="form-control" id="${key}DefaultValue" value="${json[key]}" width="40">
//                        </div>
//                    </td>
//                    <td>
//                        <button id="modalActivate" type="button" class="btn btn-danger" data-toggle="modal" data-target="#${key}Preferences">🖉 Edit Parameters</button>
//                        <div class="modal fade right" id="${key}Preferences" tabindex="-1" role="dialog" aria-labelledby="${key}PreferencesPreviewLabel" aria-hidden="true">
//                            <div class="modal-dialog modal-lg" role="document">
//                                <div class="modal-content">
//                                    <div class="modal-header">
//                                        <h5 class="modal-title" id="exampleModalPreviewLabel">Preferences for Property: ${key}</h5>
//                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
//                                            <span aria-hidden="true">&times;</span>
//                                        </button>
//                                    </div>
//                                    <div class="modal-body">
//                                        <div id="ParameterMenu">
//                                            <div class="panel-body">
//                                                <table id="${key}ParameterTable" class="table table-striped">
//                                                    <tr><th>Expected Message</th><th>HTTP Response</th><th>Random</th><th>Null</th><th>Blank</th><th>Value/Char Length</th><th>Test Name</th><th><button type="button" class="btn btn-success" onclick="addParam('${key}ParameterTable')">+ Add Parameter</button></th></tr>
//                                                </table>
//                                            </div>
//                                        </div>
//                                    </div>
//                                    <div class="modal-footer">
//                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Done</button>
//                                    </div>
//                                </div>
//                            </div>
//                        </div>
//                    </td>
//                </tr>`;
//            } // End for loop iteratively concatenating row info for attributes to table.

//            // Lastly, close off divs and the panel.
//            htmlString += `   </table>
//                </div>
//            </div>
//            <div class="panel-footer">
//                <label for="dtoName">DTO Name</label><input name="dtoName" class="form-control" />
//                <button class="btn btn-success">&#9658; Run Now</button>
//            </div>
//        </form>
//    </div>`;
//            $("#mainGUI").html(htmlString);
//            $("#jsonErrField").html("");
//        });
//    } catch (e) {
//        if (e instanceof SyntaxError) {
//            printError(e, true);
//        }
//        else {
//            printError(e, false);
//        }
//    }
//    console.timeEnd('jsonLoadTime');
//}