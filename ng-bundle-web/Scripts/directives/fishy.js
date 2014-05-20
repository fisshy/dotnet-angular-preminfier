angular.module('myApp').directive('fishy', function(){
  return {
    template : '<div>{{text}}</div>',
    link: function($scope, $element, $attr){
      $scope.text = "Hello Fishy";
    }
  };
});
