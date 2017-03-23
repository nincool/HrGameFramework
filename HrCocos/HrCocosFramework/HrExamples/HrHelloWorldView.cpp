#include "HrHelloWorldView.h"
#include "ui/UIButton.h"

using namespace Hr;
using namespace cocos2d;
using namespace cocos2d::ui;

HrHelloWorldView::HrHelloWorldView()
{

}

HrHelloWorldView::~HrHelloWorldView()
{

}

void HrHelloWorldView::HrInit()
{
	Layer::init();

	m_pClickBtnSignal = HrCheckPointerCast<HrSignal>(std::make_shared<HrHelloWorldSignal>());
	
	auto pBtn = Button::create("fishing_common_btn1_0.png", "fishing_common_btn1_1.png", "fishing_common_btn1_1.png");
	pBtn->addClickEventListener([&](Ref* pRef) {

		m_pClickBtnSignal->Dispatch(std::make_shared<HrParam>());
	});
	pBtn->setPosition(Vec2(300, 300));
	this->addChild(pBtn);
}

const HrSignalPtr& HrHelloWorldView::GetClickBtnSignal()
{
	return m_pClickBtnSignal;
}

