Angular Preminifier for ASP.NET MVC Bundling 
=========================

This is not complete and very unstable, so do not use in production yet but please contribute =)

### Usage
```C#
var js = new Bundle("~/bundles/angular").Include(
                "~/Scripts/*.js",
                "~/Scripts/controllers/*.js");


js.Transforms.Add(new AngularPreMinifierTransform());
js.Transforms.Add(new JsMinify());

bundles.Add(js);

BundleTable.EnableOptimizations = true;
```

### Input
```js
angular.module('myApp', []).run(function($rootScope){
  $rootScope.yo = "Yo Fishy!";
});
angular.module('myApp').controller('MyCntrl', function($scope){
  $scope.test = 'Hello World!';
  function name () {
    console.log("test");
  }
});
```

### Output
```js
angular.module('myApp', []).run(["$rootScope",function($rootScope){
  $rootScope.yo = "Yo Fishy!";
}]);
angular.module('myApp').controller('MyCntrl', ["$scope",function($scope){
  $scope.test = 'Hello World!';
  function name () {
    console.log("test");
  }
}]);

```

### After JsMinify
```js
angular.module("myApp",[]).run(["$rootScope",function(n){n.yo="Yo Fishy!"}]);angular.module("myApp").controller("MyCntrl",["$scope",function(n){n.test="Hello World!"}])
```


### Todo

1. Need to support injections within injections ( It's currently not grabbing the interceptors injection)
```js
angular.module('myModule')
.config(function ($httpProvider) {
  $httpProvider.interceptors.push(function ($q, $rootScope) {
  });
});
```
2. Should write unit tests.
